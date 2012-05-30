// _______________________________________________________________________________
//
//	 - WiiYourself! - native C++ Wiimote library  v1.11 BETA
//	  (c) gl.tter 2007-9 - http://gl.tter.org
//
//	  see License.txt for conditions of use.  see History.txt for change log.
// _______________________________________________________________________________
//
//  demo.cpp  (tab = 4 spaces)
#include "Demo.h"
#include "wiimote.h"
#include <mmsystem.h>	// for timeGetTime

#include <process.h>	// for _beginthreadex()

#include <windows.h>
#include <stdio.h>

#include "WiimoteServerWrapper.h"
#include "WiimotePipeServerWrapper.h"
#include "WiimoteDummyServerWrapper.h"

#include "..\FileLogger\FileLogger.h"
#include <fstream>

#define BUFSIZE 1024
#define PIPE_TIMEOUT 5000

#define MAX_WIIMOTE_CHECKS 20

// configs:
#define USE_BEEPS_AND_DELAYS			// undefine to test library works without them
#define LOOK_FOR_ADDITIONAL_WIIMOTES	// tries to connect any extra wiimotes

// ------------------------------------------------------------------------------------
//  simple callback example (we use polling for everything else):
// ------------------------------------------------------------------------------------
void on_state_change (wiimote &remote, state_change_flags changed,const wiimote_state &new_state)
	{
	// a MotionPlus was detected
	if(changed & MOTIONPLUS_DETECTED)
		{
		// enable it if there isn't a normal extension plugged into it
		// (MotionPlus devices don't report like normal extensions until
		//  enabled - and then, other extensions attached to it will no longer be
		//  reported (so disable it when you want to access to those again).
		if(remote.ExtensionType == wiimote_state::NONE) {
			bool res = remote.EnableMotionPlus();
			_ASSERT(res);
			}
		}
	else if(changed & MOTIONPLUS_EXTENSION_CONNECTED)
		{
		// an extension is connected to the MotionPlus.  We can't read it if the
		//  MotionPlus is currently enabled, so disable it:
		if(remote.MotionPlusEnabled())
			remote.DisableMotionPlus();
		}
	else if(changed & MOTIONPLUS_EXTENSION_DISCONNECTED)
		{
		// the extension plugged into the MotionPlus was removed, so enable
		//  the MotionPlus data again:
		if(remote.MotionPlusConnected())
			remote.EnableMotionPlus();
		}
	// extension was just connected:
	else if(changed & EXTENSION_CONNECTED)
		{
#ifdef USE_BEEPS_AND_DELAYS
//		Beep(1000, 200);
#endif
		// switch to a report mode that includes the extension data (we will
		//  loose the IR dot sizes)
		// note: there is no need to set report types for a Balance Board.
		if(!remote.IsBalanceBoard())
			remote.SetReportType(wiimote::IN_BUTTONS_ACCEL_IR_EXT);
		}
	// extension was just disconnected:
	else if(changed & EXTENSION_DISCONNECTED)
		{
#ifdef USE_BEEPS_AND_DELAYS
//		Beep(200, 300);
#endif
		// use a non-extension report mode (this gives us back the IR dot sizes)
		remote.SetReportType(wiimote::IN_BUTTONS_ACCEL_IR);
		}
	}
// ------------------------------------------------------------------------------------
void PrintTitle (HANDLE console)
	{
	BRIGHT_WHITE;
	_tprintf(_T("\n")); 
	_tprintf(_T("   -WiiYourself!- "));
	WHITE; _tprintf(		   _T("library Demo:   "));
	CYAN;  _tprintf(						   _T("| (c) "));
	BRIGHT_CYAN;  _tprintf(							 _T("gl"));
	BRIGHT_PURPLE;_tprintf(                            _T("."));
	BRIGHT_CYAN;  _tprintf(								_T("tter"));
	CYAN; _tprintf(										    _T(" 2007-09\n")
			 _T("                       v") WIIYOURSELF_VERSION_STR
										 _T(" | http://gl.tter.org\n"));
	CYAN;_tprintf(_T(" ______________________________________________________________________\n\n\n"));
	}


void PrintToConsole(wiimote *remote1,wiimote *remote2, HANDLE *console,DWORD last_rumble_time,DWORD current_time,bool rumble_text)
{
		// Battery level:
		CYAN; _tprintf(_T("  Battery: "));
		// (the green/yellow colour ranges are rough guesses - my wiimote
		//  with rechargeable battery pack fails at around 15%)
		(remote1->bBatteryDrained	    )? BRIGHT_RED   :
		(remote1->BatteryPercent >= 30)? BRIGHT_GREEN : BRIGHT_YELLOW;
		_tprintf(_T("%3u%%   "), remote1->BatteryPercent);


		CYAN; _tprintf(_T("LEDs: ")); WHITE; _tprintf(_T("["));
		for(unsigned led=0; led<4; led++)
			{
			if(remote1->LED.Lit(led)) {
				BRIGHT_CYAN; _tprintf(_T("*"));
				}
			else{
				WHITE      ; _tprintf(_T("-"));//_T("%c"), '0'+led);
				}
			}

		// Rumble
		WHITE; _tprintf(_T("]    "));
		if(remote1->bRumble) {
			BRIGHT_WHITE; _tprintf(rumble_text? _T(" RUMBLE") : _T("RUMBLE "));
			// animate the text
			if((current_time - last_rumble_time) >= 110) {
				rumble_text		 = !rumble_text;
				last_rumble_time = current_time;
				}
			}
		else
			_tprintf(_T("       "));

		// Output method:
	    CYAN; _tprintf( _T("    using %s\n"), (remote1->IsUsingHIDwrites()?
											   _T("HID writes") : _T("WriteFile()")));

		// Buttons:
		CYAN; _tprintf(_T("\n  Buttons: ")); WHITE; _tprintf(_T("["));
		for(unsigned bit=0; bit<16; bit++)
			{
			WORD mask = (WORD)(1 << bit);
			// skip unused bits
			if((wiimote_state::buttons::ALL & mask) == 0)
				continue;

			const TCHAR* button_name = wiimote::ButtonNameFromBit[bit];
			bool		 pressed	 = ((remote1->Button.Bits & mask) != 0);
			if(bit > 0) {
				CYAN; _tprintf(_T("|")); // seperator
				}
			if(pressed) {
				BRIGHT_WHITE; _tprintf(_T("%s")  , button_name);
				}
			else{
				WHITE       ; _tprintf(_T("%*s"), _tcslen(button_name), _T(""));
				}
			}
		WHITE; _tprintf(_T("]\n"));

		// Acceleration:
		CYAN ; _tprintf(_T("    Accel:"));
		remote1->IsBalanceBoard()? RED : WHITE;
		_tprintf(_T("  X %+2.3f  Y %+2.3f  Z %+2.3f  \n"),
					remote1->Acceleration.X,
					remote1->Acceleration.Y,
					remote1->Acceleration.Z);

	
		// Orientation estimate (shown red if last valid update is aging):
		CYAN ; _tprintf(_T("   Orient:"));
		remote1->IsBalanceBoard()? RED : WHITE;
		_tprintf(_T("  UpdateAge %3u  "), remote1->Acceleration.Orientation.UpdateAge);
		
		//  show if the last orientation update is considered out-of-date
		//   (using an arbitrary threshold)
		if(remote1->Acceleration.Orientation.UpdateAge > 10)
			RED;
			
		_tprintf(_T("Pitch:%4ddeg  Roll:%4ddeg  \n")
			     _T("                           (X %+.3f  Y %+.3f  Z %+.3f)      \n"),
				 (int)remote1->Acceleration.Orientation.Pitch,
				 (int)remote1->Acceleration.Orientation.Roll ,
				 remote1->Acceleration.Orientation.X,
				 remote1->Acceleration.Orientation.Y,
  				 remote1->Acceleration.Orientation.Z);

		// IR:
		CYAN ; _tprintf(_T("       IR:"));
		remote1->IsBalanceBoard()? RED : WHITE;
		_tprintf(_T("  Mode %s  "),
			((remote1->IR.Mode == wiimote_state::ir::OFF     )? _T("OFF  ") :
			 (remote1->IR.Mode == wiimote_state::ir::BASIC   )? _T("BASIC") :
			 (remote1->IR.Mode == wiimote_state::ir::EXTENDED)? _T("EXT. ") :
															  _T("FULL ")));
		// IR dot sizes are only reported in EXTENDED IR mode (FULL isn't supported yet)
		bool dot_sizes = (remote1->IR.Mode == wiimote_state::ir::EXTENDED);

		for(unsigned index=0; index<4; index++)
			{
			wiimote_state::ir::dot &dot = remote1->IR.Dot[index];
			
			if(!remote1->IsBalanceBoard()) WHITE;
			_tprintf(_T("%u: "), index);

			if(dot.bVisible) {
				WHITE; _tprintf(_T("Seen       "));
				}
			else{
				RED  ; _tprintf(_T("Not seen   "));
				}

			_tprintf(_T("Size"));
			if(dot_sizes)
				 _tprintf(_T("%3d "), dot.Size);
			else{
				RED; _tprintf(_T(" n/a"));
				if(dot.bVisible) WHITE;
				}

			_tprintf(_T("  X %.3f  Y %.3f\n"), dot.X, dot.Y);
			
			if(index < 3)
				_tprintf(_T("                        "));
			}

		// Speaker:
		CYAN ; _tprintf(_T("  Speaker:"));
		remote1->IsBalanceBoard()? RED : WHITE;
		_tprintf(_T("  %s | %s    "), (remote1->Speaker.bEnabled? _T("On ") :
															    _T("Off")),
									  (remote1->Speaker.bMuted  ? _T("Muted") :
																_T("     ")));
		if(!remote1->Speaker.bEnabled || remote1->Speaker.bMuted)
			RED;
		else//if(remote1->IsPlayingAudio()) BRIGHT_WHITE; else WHITE;
			WHITE;
		_tprintf(_T("Frequency %4u Hz   Volume 0x%02x\n"),
				 wiimote::FreqLookup[remote1->Speaker.Freq],
				 remote1->Speaker.Volume);

}

void PrintExtensionDataToConsole(wiimote *remote1,wiimote *remote2, HANDLE *console)
{
				// -- Extensions --:
				CYAN ; _tprintf(_T("__________\n  Extnsn.:  "));
				BRIGHT_WHITE; _tprintf(_T("Motion Plus"));

				CYAN ; _tprintf(_T("    Raw: "));
				WHITE; _tprintf(_T("Yaw: %04hx  ")   , remote1->MotionPlus.Raw.Yaw);
				WHITE; _tprintf(_T("Pitch: %04hx  ") , remote1->MotionPlus.Raw.Pitch);
				WHITE; _tprintf(_T("Roll: %04hx  \n"), remote1->MotionPlus.Raw.Roll);
				CYAN ; _tprintf(_T("                         Float: "));
				WHITE; _tprintf(_T("  %8.3fdeg")     , remote1->MotionPlus.Speed.Yaw);
				WHITE; _tprintf(_T("  %8.3fdeg")   , remote1->MotionPlus.Speed.Pitch);
				WHITE; _tprintf(_T(" %8.3fdeg\n")   , remote1->MotionPlus.Speed.Roll);
				_tprintf(BLANK_LINE BLANK_LINE);
}

// ------------------------------------------------------------------------------------
unsigned __stdcall startTUIOWrapperThread (void * params)
{
	/*
	printf("\nIn startTUIOWrapperThread");
	TUIOWrapper    &wrapper = *(TUIOWrapper*)params;
	wrapper.run();

	printf("\nStarterd TUIOWrapperThread");
	return 0;
	*/
}


void logWiimoteFileHeader(FILE *fp,int beatsPerMinute, int beatsPerBar, int numBars,int leadIn)
{
	fprintf(fp,"BPM,%d\n",beatsPerMinute);
	fprintf(fp,"BPB,%d\n",beatsPerBar);
	fprintf(fp,"NumBar,%d\n",numBars);
	fprintf(fp,"LeadIn,%d\n",leadIn);

	fprintf(fp,"\nSequence #,Time,Acc Pitch Orientation #WM1, Acc Roll Orientation #WM1,Acc X #WM1,Acc Y #WM1,Acc Z #WM1,\
		Raw Yaw #WM1,Raw Pitch #WM1,Raw Roll #WM1,Speed Yaw #WM1, Speed Pitch #WM1,Speed Roll #WM1,\
		Acc Pitch Orientation #WM2, Acc Roll Orientation #WM2,Acc X #WM2,Acc Y #WM2,Acc Z #WM2,\
		Raw Yaw #WM2,Raw Pitch #WM2,Raw Roll #WM2,Speed Yaw #WM2, Speed Pitch #WM2,Speed Roll #WM2");
}

// ------------------------------------------------------------------------------------

int initializeServer(WiimoteServerWrapper ** server,WiimoteData *wiimoteData1,WiimoteData *wiimoteData2,bool testing)
{
	if(testing)
		*server = (WiimoteServerWrapper *)new WiimoteDummyServerWrapper();
	else
		*server = (WiimoteServerWrapper *)new WiimotePipeServerWrapper();
	(*server)->startWiimoteServers(wiimoteData1,wiimoteData2);

	return 0;
}

HANDLE initializeWiimotesBeforeConnection(WiimoteData *wiimoteData1,WiimoteData *wiimoteData2)
{
	Beep(500, 30);

	SetConsoleTitle(_T("- WiiYourself! - Demo: "));
	HANDLE console = GetStdHandle(STD_OUTPUT_HANDLE);

	// write the title
//	PrintTitle(console);

	
	// simple callback example (we use polling for almost everything here):
	wiimoteData1->m_wiimote.ChangedCallback		= on_state_change;
	//  notify us only when something related to the extension changes
	wiimoteData1->m_wiimote.CallbackTriggerFlags = (state_change_flags)( EXTENSION_CHANGED |
													   MOTIONPLUS_CHANGED);


	// simple callback example (we use polling for almost everything here):
	wiimoteData2->m_wiimote.ChangedCallback		= on_state_change;
	//  notify us only when something related to the extension changes
	wiimoteData2->m_wiimote.CallbackTriggerFlags = (state_change_flags)( EXTENSION_CHANGED |
													   MOTIONPLUS_CHANGED);

	return console;
}

void cleanupOnDisconnect(WiimoteData *wiimoteData1,WiimoteData *wiimoteData2, HANDLE *console)
{
	// disconnect (auto-happens on wiimote destruction anyway, but let's play nice)
	wiimoteData1->m_wiimote.Disconnect();
	wiimoteData2->m_wiimote.Disconnect();
	Beep(1000, 200);


	BRIGHT_WHITE; // for automatic 'press any key to continue' msg
//	CloseHandle(console);

//	matlab_terminate();
//	getchar();
}


int connectWiimotes(WiimoteServerWrapper * server,WiimoteData *wiimoteData1,WiimoteData *wiimoteData2,WiimoteData *wiimoteData3,HANDLE *console)
{

DWORD current_time;

//reconnect:/
	COORD pos = { 0, 6 };
	SetConsoleCursorPosition(console, pos);

	// try to connect the first available wiimote in the system
	//  (available means 'installed, and currently Bluetooth-connected'):
	WHITE; _tprintf(_T("  Looking for a Wiimote     "));
	   
	static const TCHAR* wait_str[] = { _T(".  "), _T(".. "), _T("...") };
	unsigned count = 0;

	while(!wiimoteData1->m_wiimote.Connect(wiimote::FIRST_AVAILABLE)) {
		_tprintf(_T("\b\b\b\b%s "), wait_str[count%3]);
		count++;
#ifdef USE_BEEPS_AND_DELAYS
//		Beep(500, 30); 
#endif
		}

		//while(wiimoteData1->m_wiimote.bWiimoteDetectionComplete != true)
		//	Sleep(1000);

	while(!wiimoteData2->m_wiimote.Connect(2)) {
		_tprintf(_T("\b\b\b\b%s "), wait_str[count%3]);
		count++;
#ifdef USE_BEEPS_AND_DELAYS
//		Beep(500, 30); 
		Sleep(1000);
#endif
		}
				Sleep(5000);												//Sachin , Sleep	

	count = 0;
	while(!wiimoteData3->m_wiimote.Connect(3)) {
		_tprintf(_T("\b\b\b\b%s "), wait_str[count%3]);
		count++;
		if(count == 10)
			break;
#ifdef USE_BEEPS_AND_DELAYS
//		Beep(500, 30); 
		Sleep(1000);
#endif
		}
				Sleep(5000);					

	// connected - light all LEDs
	wiimoteData1->m_wiimote.SetLEDs(0x0f);
	wiimoteData2->m_wiimote.SetLEDs(0x0f);
	wiimoteData1->setConnectionState(WIIMOTE_CONNECTED_STATE);
	wiimoteData2->setConnectionState(WIIMOTE_CONNECTED_STATE);
    server->setWiimoteConnectionState(wiimoteData1,wiimoteData2);

	BRIGHT_CYAN; _tprintf(_T("\b\b\b\b... connected!"));
	current_time = timeGetTime();
//	tuioWrapper->sendMessage(WIIMOTES_CONNECTED_CODE,current_time,WIIMOTES_CONNECTED_CODE,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0);

#ifdef USE_BEEPS_AND_DELAYS
//	Beep(1000, 300); 
	Sleep(2000);
#endif

		return 0;
}

int initializeWiimotesAfterConnection(WiimoteData *wiimoteData1,WiimoteData *wiimoteData2,HANDLE *console)
{

	// ask the wiimote to report everything (using the 'non-continous updates'
	//  default mode - updates will be frequent anyway due to the acceleration/IR
	//  values changing):

	// note1: you don't need to set a report type for Balance Board - the library
	//         takes care of it (there is only one).
	// note2: for wiimotes, the report mode that includes the extension data
	//		   unfortunately only reports the 'BASIC' IR info (ie. no dot sizes) -
	//		   so let's choose the best mode based on the extension status (we also
	//		   toggle modes as needed in the callback above):
	if(!wiimoteData1->m_wiimote.IsBalanceBoard())
		{
		if(wiimoteData1->m_wiimote.bExtension)
			wiimoteData1->m_wiimote.SetReportType(wiimote::IN_BUTTONS_ACCEL_IR_EXT); // no IR dots
		else
			wiimoteData1->m_wiimote.SetReportType(wiimote::IN_BUTTONS_ACCEL_IR);		//    IR dots
		}

	if(!wiimoteData2->m_wiimote.IsBalanceBoard())
		{
		if(wiimoteData2->m_wiimote.bExtension)
			wiimoteData2->m_wiimote.SetReportType(wiimote::IN_BUTTONS_ACCEL_IR_EXT); // no IR dots
		else
			wiimoteData2->m_wiimote.SetReportType(wiimote::IN_BUTTONS_ACCEL_IR);		//    IR dots
		}

		return 0;
}

int startWiimoteRead(WiimoteServerWrapper * server,WiimoteData *wiimoteData1,WiimoteData *wiimoteData2,HANDLE *console)
{

	// print the button event instructions:
	//BRIGHT_WHITE;
	//_tprintf(_T("\r  -- TRY: B = rumble, A = square, 1 = sine, 2 = daisy, Home = Exit --\n"));

	// (stuff for animations)
	DWORD	 last_rumble_time = timeGetTime(); // for rumble text animation
	DWORD    last_led_time    = timeGetTime(); // for led         animation
	bool	 rumble_text	  = true;
	unsigned lit_led          = 0;	


	DWORD	 init_time = timeGetTime(); // Init time

	COORD cursor_pos = { 0, 6 };

	DWORD current_time;

	int sequenceNumber = 0;

	//To check Wiimote Health in the start
	int wiimote1MotionPlusChanged = 0;
	int wiimote2MotionPlusChanged = 0;
	int wiiMotionPlusChangeCounter = 0;
	float wiimote1YawValue = 0;
	float wiimote1RollValue = 0;
	float wiimote1PitchValue = 0;
	float wiimote2YawValue = 0;
	float wiimote2RollValue = 0;
	float wiimote2PitchValue = 0;

	// display the wiimote state data until 'Home' is pressed:
	bool caliubrate_atrest_once = true;
//	while(!wiimoteData1->m_wiimote.Button.Home())// && !GetAsyncKeyState(VK_ESCAPE))
	while(1)
		{

//			if(server->isWiimoteLoggingOn())
//			{
//				server->startLogging(wiimoteData1,wiimoteData2);
//				init_time = timeGetTime();
//			}
		if(wiimoteData1->m_wiimote.Button.Home())
		{
			cleanupOnDisconnect(wiimoteData1,wiimoteData2,console);
			exit(0);
		}

		Sleep(server->getFrequencyInterval());
		// the wiimote state needs to be refreshed for each pass
		while(wiimoteData1->m_wiimote.RefreshState() == NO_CHANGE && wiimoteData2->m_wiimote.RefreshState() == NO_CHANGE)
			Sleep(1); // // don't hog the CPU if nothing changed

		cursor_pos.Y = 8;
		SetConsoleCursorPosition(console, cursor_pos);

		// did we loose the connection?
		if(wiimoteData1->m_wiimote.ConnectionLost() || wiimoteData2->m_wiimote.ConnectionLost())
			{
			BRIGHT_RED; _tprintf(
				_T("   *** connection lost! ***                                          \n")
				BLANK_LINE BLANK_LINE BLANK_LINE BLANK_LINE BLANK_LINE BLANK_LINE
				BLANK_LINE BLANK_LINE BLANK_LINE BLANK_LINE BLANK_LINE BLANK_LINE
				BLANK_LINE BLANK_LINE BLANK_LINE);
			Beep(100, 1000);
			Sleep(2000);
			COORD pos = { 0, 6 };
			SetConsoleCursorPosition(console, pos);
			_tprintf(BLANK_LINE BLANK_LINE BLANK_LINE);
			wiimoteData1->setConnectionState(WIIMOTE_DISCONNECTED_STATE);
			wiimoteData2->setConnectionState(WIIMOTE_DISCONNECTED_STATE);
			server->setWiimoteConnectionState(wiimoteData1,wiimoteData2);
//			fclose(fp);
			return -1;
			}

		// rumble if 'B' (trigger) is pressed
		wiimoteData1->m_wiimote.SetRumble(wiimoteData1->m_wiimote.Button.B());
		wiimoteData2->m_wiimote.SetRumble(wiimoteData2->m_wiimote.Button.B());

		// disable MotionPlus (if connected) to allow its own extension port
		//  to work
		if(wiimoteData1->m_wiimote.Button.Minus())
			wiimoteData1->m_wiimote.DisableMotionPlus();
		if(wiimoteData2->m_wiimote.Button.Minus())
			wiimoteData2->m_wiimote.DisableMotionPlus();

		current_time = timeGetTime();

		// LEDs:
		//  animate them every second
		if((current_time - last_led_time) >= 1000) {
			wiimoteData1->m_wiimote.SetLEDs((BYTE)(1<<lit_led));
			lit_led		  = (++lit_led) % 4;
			last_led_time = timeGetTime();
			}

//		PrintToConsole(remote1,remote2,console,last_rumble_time,current_time,rumble_text);

		current_time = timeGetTime() - init_time; // Init time

		switch(wiimoteData1->m_wiimote.ExtensionType)
			{
			case wiimote_state::NONE:
				{
/*				RED;
				_tprintf(_T("None                                                             \n"));
				_tprintf(BLANK_LINE BLANK_LINE BLANK_LINE);*/
				}
				break;
			
			case wiimote_state::PARTIALLY_INSERTED:
				{
				BRIGHT_RED;
				_tprintf(_T("Partially Inserted                                               \n"));
				_tprintf(BLANK_LINE BLANK_LINE BLANK_LINE);
				}
				break;
			
			case wiimote_state::MOTION_PLUS:
				{
//					PrintExtensionDataToConsole(remote1,remote2,console);

				if(server->sendWiimoteData(wiimoteData1,wiimoteData2,sequenceNumber,current_time))
				{
					sequenceNumber = 0; //This indicates that logging is restarted for a new recording
					init_time = timeGetTime();
				}

			if(wiimote1MotionPlusChanged == 0)
			{
				if(wiimote1YawValue == 0)
				{
					wiimote1YawValue = wiimoteData1->m_wiimote.MotionPlus.Speed.Yaw;
					wiimote1RollValue = wiimoteData1->m_wiimote.MotionPlus.Speed.Roll;
					wiimote1PitchValue = wiimoteData1->m_wiimote.MotionPlus.Speed.Pitch;
				}
				else if(wiimote1YawValue != wiimoteData1->m_wiimote.MotionPlus.Speed.Yaw)
					wiimote1MotionPlusChanged = 1;
				else if(wiimote1RollValue != wiimoteData1->m_wiimote.MotionPlus.Speed.Roll)
					wiimote1MotionPlusChanged = 1;
				else if(wiimote1PitchValue != wiimoteData1->m_wiimote.MotionPlus.Speed.Pitch)
					wiimote1MotionPlusChanged = 1;
				else
					wiiMotionPlusChangeCounter ++;
			}

			FILE * fp = fopen("WiimoteGyroDataDump1.log","a");
			fprintf(fp,"\n%d,%f,%f,%f",wiimote1MotionPlusChanged,wiimoteData1->m_wiimote.MotionPlus.Speed.Yaw,wiimoteData1->m_wiimote.MotionPlus.Speed.Roll,wiimoteData1->m_wiimote.MotionPlus.Speed.Pitch);
			fclose(fp);

			fp = fopen("WiimoteGyroDataDump2.log","a");
			fprintf(fp,"\n%d,%f,%f,%f",wiimote2MotionPlusChanged,wiimoteData2->m_wiimote.MotionPlus.Speed.Yaw,wiimoteData2->m_wiimote.MotionPlus.Speed.Roll,wiimoteData2->m_wiimote.MotionPlus.Speed.Pitch);
			fclose(fp);

			if(wiimote2MotionPlusChanged == 0)
			{
				if(wiimote2YawValue == 0)
				{
					wiimote2YawValue = wiimoteData2->m_wiimote.MotionPlus.Speed.Yaw;
					wiimote2RollValue = wiimoteData2->m_wiimote.MotionPlus.Speed.Roll;
					wiimote2PitchValue = wiimoteData2->m_wiimote.MotionPlus.Speed.Pitch;
				}
				else if(wiimote2YawValue != wiimoteData2->m_wiimote.MotionPlus.Speed.Yaw)
					wiimote2MotionPlusChanged = 1;
				else if(wiimote2RollValue != wiimoteData2->m_wiimote.MotionPlus.Speed.Roll)
					wiimote2MotionPlusChanged = 1;
				else if(wiimote2PitchValue != wiimoteData2->m_wiimote.MotionPlus.Speed.Pitch)
					wiimote2MotionPlusChanged = 1;
				else
					wiiMotionPlusChangeCounter ++;
			}

			if(wiimote1MotionPlusChanged == 1 && wiimote2MotionPlusChanged ==1)
			{
				wiimoteData1->setConnectionState(WIIMOTE_GOOD_DATA_STATE);	
				wiimoteData2->setConnectionState(WIIMOTE_GOOD_DATA_STATE);
				server->setWiimoteConnectionState(wiimoteData1,wiimoteData2);
//				FileLogger::getFileLogger()->logEntry("Setting Wiimote State to WIIMOTE_GOOD_DATA_STATE");
			}

			if(wiiMotionPlusChangeCounter > MAX_WIIMOTE_CHECKS * 2)
			{
				FileLogger::getFileLogger()->logEntry("Setting Wiimote State to WIIMOTE_BAD_DATA_STATE");

				wiimoteData1->setConnectionState(WIIMOTE_BAD_DATA_STATE);	
				wiimoteData2->setConnectionState(WIIMOTE_BAD_DATA_STATE);
				server->setWiimoteConnectionState(wiimoteData1,wiimoteData2);
				return -1;

//				exit(0);
			}

//			TCHAR msg[256];
//			swprintf(msg,_T("wiiMotionPlusChangeCounter = %d"),wiiMotionPlusChangeCounter);
//			FileLogger::getFileLogger()->logEntry(msg);

//			if(!server->isWiimoteLoggingOn())
//			{					
//				sequenceNumber = 0;
//			}


//			if(server->isWiimoteRecordingOn())
//				sequenceNumber++;

			if(sequenceNumber == 30000)
				sequenceNumber = 0;
//Sleep(500);
			}
			break;
		}
  	}

//	server->stopLogging();
	return 0;
}

void setWiimoteValueWiimotes(wiimote *remote)
{
	remote->Acceleration.Orientation.Pitch = .1;
	remote->Acceleration.Orientation.Roll =.2;
	remote->Acceleration.Orientation.X = .3;
	remote->Acceleration.Orientation.Y = .4;
	remote->Acceleration.Orientation.Z = .5;
	remote->MotionPlus.Raw.Yaw = .01;
	remote->MotionPlus.Raw.Pitch = .02;
	remote->MotionPlus.Raw.Roll = .03;
	remote->MotionPlus.Speed.Yaw = .04;
	remote->MotionPlus.Speed.Pitch = .05;
	remote->MotionPlus.Speed.Roll = .06;
}

void openSimulateFile(WiimoteServerWrapper * server,ifstream **fpSimulation)
{
	*fpSimulation = new ifstream(server->getWiimoteSimulationFileName());

	char * tempLine = new char[BUFSIZE];

	for(int i = 0 ; i < 13; i++)
		(*fpSimulation)->getline(tempLine, BUFSIZE);

	delete tempLine;
}

void simulateWiimotes(WiimoteServerWrapper * server,WiimoteData *remote1,WiimoteData* remote2)
{
	int sequenceNumber = 0;
	DWORD current_time;
	DWORD	 init_time;
	ifstream *fpSimulation;
	fpSimulation = NULL;


	remote1->setConnectionState(WIIMOTE_GOOD_DATA_STATE);	
	remote2->setConnectionState(WIIMOTE_GOOD_DATA_STATE);
	server->setWiimoteConnectionState(remote1,remote2);

	openSimulateFile(server,&fpSimulation);
	init_time = timeGetTime();

	while(true)
	{

		Sleep(4.4);//WYtry this will give a 4.95 interval between data, similar to real wiimote

		current_time = timeGetTime() - init_time; // Init time
		


			if(fpSimulation != NULL && !fpSimulation->eof())
			{
				char line[BUFSIZE];
				char fileValues[24][BUFSIZE];

				fpSimulation->getline(line, BUFSIZE);
		
				sscanf(line, "%[^','],%[^','],%[^','],%[^','],%[^','],%[^','],\
					%[^','],%[^','],%[^','],%[^','],%[^','],%[^','],\
					%[^','],%[^','],%[^','],%[^','],%[^','],%[^','],\
					%[^','],%[^','],%[^','],%[^','],%[^','],%s", fileValues[0],fileValues[1],fileValues[2],fileValues[3],fileValues[4],fileValues[5],
					fileValues[6],fileValues[7],fileValues[8],fileValues[9],fileValues[10],fileValues[11],
					fileValues[12],fileValues[13],fileValues[14],fileValues[15],fileValues[16],fileValues[17],
					fileValues[18],fileValues[19],fileValues[20],fileValues[21],fileValues[22],fileValues[23]);


//				sequenceNumber = atoi(fileValues[0]);
//				current_time = atoi(fileValues[1]);
				remote1->m_wiimote.Acceleration.Orientation.Pitch = atof(fileValues[2]);
				remote1->m_wiimote.Acceleration.Orientation.Roll = atof(fileValues[3]);
				remote1->m_wiimote.Acceleration.X = atof(fileValues[4]);
				remote1->m_wiimote.Acceleration.Y = atof(fileValues[5]);
				remote1->m_wiimote.Acceleration.Z = atof(fileValues[6]);
				remote1->m_wiimote.MotionPlus.Raw.Yaw = atof(fileValues[7]);
				remote1->m_wiimote.MotionPlus.Raw.Pitch = atof(fileValues[8]);
				remote1->m_wiimote.MotionPlus.Raw.Roll = atof(fileValues[9]);
				remote1->m_wiimote.MotionPlus.Speed.Yaw = atof(fileValues[10]);
				remote1->m_wiimote.MotionPlus.Speed.Pitch = atof(fileValues[11]);
				remote1->m_wiimote.MotionPlus.Speed.Roll = atof(fileValues[12]);
				remote2->m_wiimote.Acceleration.Orientation.Pitch = atof(fileValues[13]);
				remote2->m_wiimote.Acceleration.Orientation.Roll = atof(fileValues[14]);
				remote2->m_wiimote.Acceleration.X = atof(fileValues[15]);
				remote2->m_wiimote.Acceleration.Y = atof(fileValues[16]);
				remote2->m_wiimote.Acceleration.Z = atof(fileValues[17]);
				remote2->m_wiimote.MotionPlus.Raw.Yaw = atof(fileValues[18]);
				remote2->m_wiimote.MotionPlus.Raw.Pitch = atof(fileValues[19]);
				remote2->m_wiimote.MotionPlus.Raw.Roll = atof(fileValues[20]);
				remote2->m_wiimote.MotionPlus.Speed.Yaw = atof(fileValues[21]);
				remote2->m_wiimote.MotionPlus.Speed.Pitch = atof(fileValues[22]);
				remote2->m_wiimote.MotionPlus.Speed.Roll = atof(fileValues[23]);
			}
			else
			{
				fpSimulation->close();
				openSimulateFile(server,&fpSimulation);
			}

		if(server->sendWiimoteData(remote1,remote2,sequenceNumber,current_time))
		{
			sequenceNumber = 0; //This indicates that logging is restarted for a new recording
			init_time = timeGetTime();
		}

//		if(server->isWiimoteRecordingOn())
//			sequenceNumber++;

		if(sequenceNumber == 50000)
			sequenceNumber = 0;
	}
}



int _tmain (int argc , _TCHAR* argv[])
//extern "C" __declspec(dllexport) int startWiimote()
	{
//	matlab_init();

	FileLogger::getFileLogger()->logEntry("################ Starting Wiimote C++ Application ##################");

	FILE * fp = fopen("WiimoteDataDump.log","w+");
	fclose(fp);

	fp = fopen("WiimoteMotionPlusDataDump.log","w+");
	fclose(fp);

	fp = fopen("WiimoteGyroDataDump1.log","w+");
	fclose(fp);

	fp = fopen("WiimoteGyroDataDump2.log","w+");
	fclose(fp);

	bool testing = true;

	if(argc >= 2)
	{
		if(wcscmp(argv[1],_T("true")) == 0)
			testing = true;
		_tprintf(_T("Param ='%s' \n"), argv[1]);

	}

	// create a wiimote object

	WiimoteData wiimoteData1;
	WiimoteData wiimoteData2;
	WiimoteData wiimoteData3;

	wiimoteData1.m_wiimote.setWiimoteNumber(1);
	wiimoteData2.m_wiimote.setWiimoteNumber(2);
	wiimoteData3.m_wiimote.setWiimoteNumber(3);

	WiimoteServerWrapper * server = NULL;
	int status;

	initializeServer(&server,&wiimoteData1,&wiimoteData2,testing);

	if(server->isWiimoteSimulationModeOn())
	{
		simulateWiimotes(server,&wiimoteData1,&wiimoteData2);
	}
	else
	{
		do
		{
			HANDLE console = initializeWiimotesBeforeConnection(&wiimoteData1,&wiimoteData2);
			status = connectWiimotes(server,&wiimoteData1,&wiimoteData2,&wiimoteData3,&console);
			if(status != -1)
			{
				initializeWiimotesAfterConnection(&wiimoteData1,&wiimoteData2, &console);
				status = startWiimoteRead(server,&wiimoteData1,&wiimoteData2,&console);
				CYAN ; _tprintf(_T("Failed Connection.. Retrying.."));
				FileLogger::getFileLogger()->logEntry("Failed Connection.. Retrying..");
				cleanupOnDisconnect(&wiimoteData1,&wiimoteData2,&console);
			}
		}while(status == -1);
	}

	_tprintf(_T("End main.."));

}
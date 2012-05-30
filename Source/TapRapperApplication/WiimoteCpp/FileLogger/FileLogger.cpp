
#include "stdafx.h"
#include "FileLogger.h"

#include "..\ProjectCommon\TimeData.h"

FileLogger * FileLogger::fileLogger;

FileLogger::FileLogger()
{
	logfp = fopen("WiimoteDebug.log","w+");
}


void FileLogger::logEntry(const char * msg)
{
	char locationTag[] = "None";
	logEntry(msg,locationTag);

//	TimeData time;
//	time.setToCurrentTime();
//	fprintf(logfp,"\n%d:%d:%d:%d:\t%s",time.hours,time.mins,time.seconds,time.miliseconds,msg);
}

void FileLogger::logEntry(const char * msg,const char * locationTag)
{
	TimeData time;
	time.setToCurrentTime();

	fprintf(logfp,"\n%d:%d:%d:%d:\t%s\t%s",time.hours,time.mins,time.seconds,time.miliseconds,locationTag,msg);
}


void FileLogger::logEntry(TCHAR * msg)
{
	TCHAR locationTag[] = _T("None");
	logEntry(msg,locationTag);

//	TimeData time;
//	time.setToCurrentTime();
//	fwprintf(logfp,_T("\n%d:%d:%d:%d:\t%s"),time.hours,time.mins,time.seconds,time.miliseconds,msg);
}

void FileLogger::logEntry(TCHAR * msg,TCHAR * locationTag)
{
	TimeData time;
	time.setToCurrentTime();
	fwprintf(logfp,_T("\n%d:%d:%d:%d:\t%s\t%s"),time.hours,time.mins,time.seconds,time.miliseconds,locationTag,msg);
//	fflush(logfp);

}




void FileLogger::closeFile()
{
	fclose(logfp);
}


FileLogger::~FileLogger()
{
	fclose(logfp);
}

FileLogger * FileLogger::getFileLogger()
{
	if(fileLogger == NULL)
		fileLogger = new FileLogger();
	return fileLogger;
}
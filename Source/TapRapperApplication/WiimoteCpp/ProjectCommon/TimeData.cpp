#include "stdafx.h"
#include "TimeData.h"

void TimeData::setToCurrentTime()
{
	currentTime = timeGetTime();
	hours = currentTime/(60*60*1000);
	mins = (currentTime%(60*60*1000))/(60*1000);
	seconds = ((currentTime% (1000*60*60)) % (1000*60)) / 1000;
	miliseconds = currentTime%1000;
}
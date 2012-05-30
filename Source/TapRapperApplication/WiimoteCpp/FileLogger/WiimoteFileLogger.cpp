
#include "stdafx.h"
#include "WiimoteFileLogger.h"
#include <stdio.h>


void WiimoteFileLogger::logEntry(const char * msg)
{
	fprintf(logfp,"\n%s",msg);
}

void WiimoteFileLogger::logEntry(TCHAR * msg)
{
	fwprintf(logfp,_T("\"%s\"\n"),msg);
}

WiimoteFileLogger * WiimoteFileLogger::getWiimoteFileLogger()
{
	if(fileLogger == NULL)
		fileLogger = (FileLogger *)new FileLogger();
	return (WiimoteFileLogger * )fileLogger;
}
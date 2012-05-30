
#include <stdio.h>
#include <tchar.h>
#include "FileLogger.h"

#ifndef WIIMMOTE_FILE_LOGGER_H
#define WIIMMOTE_FILE_LOGGER_H

class WiimoteFileLogger : public FileLogger
{
public :

	static WiimoteFileLogger * getWiimoteFileLogger();
	virtual void logEntry(const char * msg);
	virtual void logEntry(TCHAR * msg);
	void closeFile();
	~WiimoteFileLogger() {}

private :
	FILE *logfp;
	static FileLogger * fileLogger;
};

#endif
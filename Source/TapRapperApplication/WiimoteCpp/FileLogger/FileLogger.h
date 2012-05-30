
#include <stdio.h>
#include <tchar.h>

#ifndef _FILE_LOGGER_H
# define _FILE_LOGGER_H

class FileLogger
{
public :

	FileLogger();
	static FileLogger* getFileLogger();
	void logEntry(const char * msg);
	void logEntry(const char * msg,const char * locationTag);
	void logEntry(TCHAR * msg);
	void logEntry(TCHAR * msg,TCHAR * locationTag);
	void closeFile();
	~FileLogger();

private :
	FILE *logfp;
	static FileLogger * fileLogger;
};

#endif
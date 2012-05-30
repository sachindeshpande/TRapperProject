#ifndef TIME_DATA_H
# define TIME_DATA_H

#include <Windows.h>
#include <mmsystem.h>	// for timeGetTime

class TimeData
{
public :
	unsigned long currentTime;
	unsigned long hours;
	unsigned long mins;
	unsigned long seconds;
	unsigned long miliseconds;

	void setToCurrentTime();
};



#endif

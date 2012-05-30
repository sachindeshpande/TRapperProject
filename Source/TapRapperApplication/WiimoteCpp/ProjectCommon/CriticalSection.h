#ifndef CRITICAL_SECTION_H
# define CRITICAL_SECTION_H

#include <windows.h>

class CriticalSection
{
private:
	CRITICAL_SECTION cs;
public:
	CriticalSection();
	~CriticalSection();
	void Enter();
	void Leave();

};




#endif

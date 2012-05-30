
#include "stdafx.h"
#include "CriticalSection.h"


CriticalSection::CriticalSection()
{
	InitializeCriticalSectionAndSpinCount(&cs,10);
}

CriticalSection::~CriticalSection()
{
	DeleteCriticalSection(&cs);
}


void CriticalSection::Enter()
{
	    /* Enter the critical section -- other threads are locked out */
	   EnterCriticalSection(&cs);
}

void CriticalSection::Leave()
{

		/* Leave the critical section -- other threads can now EnterCriticalSection() */
		LeaveCriticalSection(&cs);
}

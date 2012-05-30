#ifndef SYNCHRONIZED_QUEUE_H
# define SYNCHRONIZED_QUEUE_H


#include <queue>
#include <vector>
#include "CriticalSection.h"
#include "DoubleArrayDataPacket.h"


class SynchronizedQueue
{
public :
	SynchronizedQueue();
	~SynchronizedQueue();

	void push( void * value);
	void * popAndReturn();
	int size();

private :
	std::queue<void *> queueObject;
	CriticalSection cs;

	void pop();
	void * front();

};

#endif;
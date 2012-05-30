#include "stdafx.h"
#include "SynchronizedQueue.h"

SynchronizedQueue::SynchronizedQueue()
{
}

SynchronizedQueue::~SynchronizedQueue()
{
}

void SynchronizedQueue::push( void * value)
{
	cs.Enter();
	queueObject.push(value);
	cs.Leave();
}

void SynchronizedQueue::pop()
{
	queueObject.pop();
}

void * SynchronizedQueue::front()
{
	return queueObject.front();
}

void * SynchronizedQueue::popAndReturn()
{
	while(size() == 0)
		Sleep(1);

	cs.Enter();
	void * pointer = front();
	pop();
	cs.Leave();
	return pointer;
}

int SynchronizedQueue::size()
{
	return queueObject.size();
}


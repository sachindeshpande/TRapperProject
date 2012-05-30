#ifndef PROJECT_CONSTANTS_H
# define PROJECT_CONSTANTS_H

static CRITICAL_SECTION cs; /* This is the critical section object -- once initialized,
                               it cannot be moved in memory */
                            /* If you program in OOP, declare this in your class */

const int WIIMOTE_DATA_PACKET_SIZE = 22;
const int WIIMOTE_IR_DATA_PACKET_SIZE = 38;

#define DATA_BUFFER_SIZE	16384 // 4K bytes

const int DATA_TRANSMISSION_END_TAG = -9999;

#endif

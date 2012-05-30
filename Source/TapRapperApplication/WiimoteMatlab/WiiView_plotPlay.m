function WiiView_plotPlay(hplay, data, offset)
set(hplay.hwii1AX, 'xdata', data.time);
set(hplay.hwii1AX, 'ydata', data.wii1.ax+5+offset);
set(hplay.hwii1AY, 'xdata', data.time);
set(hplay.hwii1AY, 'ydata', data.wii1.ay+5+offset);
set(hplay.hwii1AZ, 'xdata', data.time);
set(hplay.hwii1AZ, 'ydata', data.wii1.az+5+offset);
set(hplay.hwii1Pitch, 'xdata', data.time);
set(hplay.hwii1Pitch, 'ydata', data.wii1.pitch/100+2.5+offset);
set(hplay.hwii1Roll, 'xdata', data.time);
set(hplay.hwii1Roll, 'ydata', data.wii1.roll/100+2.5+offset);
set(hplay.hwii1Yaw, 'xdata', data.time);
set(hplay.hwii1Yaw, 'ydata', data.wii1.yaw/100+2.5+offset);

set(hplay.hwii2AX, 'xdata', data.time);
set(hplay.hwii2AX, 'ydata', data.wii2.ax-2.5+offset);
set(hplay.hwii2AY, 'xdata', data.time);
set(hplay.hwii2AY, 'ydata', data.wii2.ay-2.5+offset);
set(hplay.hwii2AZ, 'xdata', data.time);
set(hplay.hwii2AZ, 'ydata', data.wii2.az-2.5+offset);
set(hplay.hwii2Pitch, 'xdata', data.time);
set(hplay.hwii2Pitch, 'ydata', data.wii2.pitch/100-5+offset);
set(hplay.hwii2Roll, 'xdata', data.time);
set(hplay.hwii2Roll, 'ydata', data.wii2.roll/100-5+offset);
set(hplay.hwii2Yaw, 'xdata', data.time);
set(hplay.hwii2Yaw, 'ydata', data.wii2.yaw/100-5+offset);

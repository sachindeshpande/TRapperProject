function showwiidata(data, offset)
plot(data.time, data.wii1.ax+5+offset, 'r');
hold on
plot(data.time, data.wii1.ay+5+offset, 'g');
plot(data.time, data.wii1.az+5+offset, 'b');

plot(data.time, data.wii1.pitch/100+2.5+offset, 'r');
plot(data.time, data.wii1.roll/100+2.5+offset, 'g');
plot(data.time, data.wii1.yaw/100+2.5+offset, 'b');

plot(data.time, data.wii2.ax-2.5+offset, 'r');
hold on
plot(data.time, data.wii2.ay-2.5+offset, 'g');
plot(data.time, data.wii2.az-2.5+offset, 'b');

plot(data.time, data.wii2.pitch/100-5+offset, 'r');
plot(data.time, data.wii2.roll/100-5+offset, 'g');
plot(data.time, data.wii2.yaw/100-5+offset, 'b');

axis([data.time(1), data.time(end), -10-abs(offset), 10+abs(offset)])
grid on

end
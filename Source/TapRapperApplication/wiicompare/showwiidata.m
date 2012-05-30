function showwiidata(data)
clf
subplot(2,2,1);
plot(data.time, data.wii1.ax, 'r');
hold on
plot(data.time, data.wii1.ay, 'g');
plot(data.time, data.wii1.az, 'b');
grid on
ax=axis;
x0=ax(1); x1=ax(2);
axis([x0, x1, -5, 5]);
ylabel('Wii1 a');

subplot(2,2,3);
plot(data.time, data.wii2.ax, 'r');
hold on
plot(data.time, data.wii2.ay, 'g');
plot(data.time, data.wii2.az, 'b');
grid on
axis([x0, x1, -5, 5]);
ylabel('Wii2 a');

subplot(2,2,2);
plot(data.time, data.wii1.yaw, 'r');
hold on
plot(data.time, data.wii1.pitch, 'g');
plot(data.time, data.wii1.roll, 'b');
grid on
axis([x0, x1, -500, 500]);
ylabel('Wii1 w');

subplot(2,2,4);
plot(data.time, data.wii2.yaw, 'r');
hold on
plot(data.time, data.wii2.pitch, 'g');
plot(data.time, data.wii2.roll, 'b');
grid on
axis([x0, x1, -500, 500]);
ylabel('Wii2 w');
% t=data.time; clear tmean
% t0=2; tdelta=10;
% t1=t0+tdelta; tcnt=1;
% while(t1<length(t))
%     tmean(tcnt)=sum(t(t0:t1))/(tdelta+1);
%     tcnt=tcnt+1;
%     t0=t0+2; t1=t0+tdelta;
% end
% clf
% plot(tmean)



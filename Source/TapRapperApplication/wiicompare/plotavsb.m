

figNum=2;
figure(figNum); clf;
scalea=5; scalew=5;
data=data1b;
%TimeSeq=double(data.time-BeatLen*data.BPB*(data.LeadIn-1))/BeatLen/data.BPB;
TimeSeq=tsequence;
 subplot(8,1,1)
plot(TimeSeq, data(1,:), 'r');hold on
plot(TimeSeq, data(2,:), 'g');
plot(TimeSeq, data(3,:), 'b');
axis([TimeSeq(1) TimeSeq(end) -scalea scalea]); title(['Reference, ', fnamerefstr]);
ylabel([Crefwii1,'a']); grid on;
%legend('x1', 'y1', 'z1');
subplot(8,1,3)
plot(TimeSeq, data(7,:), 'r');hold on
plot(TimeSeq, data(8,:), 'g');
plot(TimeSeq, data(9,:), 'b');
axis([TimeSeq(1) TimeSeq(end) -scalea scalea]);
ylabel([Crefwii2,'a']);grid on;
%legend('x2', 'y2', 'z2');
subplot(8,1,5)
plot(TimeSeq, data(6,:), 'r'); hold on %yaw
plot(TimeSeq, data(4,:), 'g'); %pitch
plot(TimeSeq, data(5,:), 'b'); %roll
axis([TimeSeq(1) TimeSeq(end) -scalew scalew]);
ylabel([Crefwii1, 'w']);grid on;
%legend('yaw2', 'pitch2', 'roll2');
subplot(8,1,7)
plot(TimeSeq, data(12,:), 'r'); hold on
plot(TimeSeq, data(10,:), 'g');
plot(TimeSeq, data(11,:), 'b');
axis([TimeSeq(1) TimeSeq(end) -scalew scalew]);
ylabel([Crefwii2, 'w']);grid on;
%legend('yaw2', 'pitch2', 'roll2');

data=data2b;
 subplot(8,1,2)
plot(TimeSeq, data(1,:), 'r');hold on
plot(TimeSeq, data(2,:), 'g');
plot(TimeSeq, data(3,:), 'b');
axis([TimeSeq(1) TimeSeq(end) -scalea scalea]); title(['Play, ', fnamerefstr]);
ylabel([Crefwii1,'a']); grid on;
%legend('x1', 'y1', 'z1');
subplot(8,1,4)
plot(TimeSeq, data(7,:), 'r');hold on
plot(TimeSeq, data(8,:), 'g');
plot(TimeSeq, data(9,:), 'b');
axis([TimeSeq(1) TimeSeq(end) -scalea scalea]);
ylabel([Crefwii2,'a']);grid on;
%legend('x2', 'y2', 'z2');
subplot(8,1,6)
plot(TimeSeq, data(6,:), 'r'); hold on %yaw
plot(TimeSeq, data(4,:), 'g'); %pitch
plot(TimeSeq, data(5,:), 'b'); %roll
axis([TimeSeq(1) TimeSeq(end) -scalew scalew]);
ylabel([Crefwii1, 'w']);grid on;
%legend('yaw2', 'pitch2', 'roll2');
subplot(8,1,8)
plot(TimeSeq, data(12,:), 'r'); hold on
plot(TimeSeq, data(10,:), 'g');
plot(TimeSeq, data(11,:), 'b');
axis([TimeSeq(1) TimeSeq(end) -scalew scalew]);
ylabel([Crefwii2, 'w']);grid on;
%legend('yaw2', 'pitch2', 'roll2');


%plot ref vs play

idx=find(reference=='\');
fnamerefstr=reference((idx(end)+1):end);

idx=find(fnamerefstr=='_');
if(length(idx>0))
    for i1=1:length(idx)
        fnamerefstr(idx(i1))=' ';
    end
end

idx=find(fnameplay=='\');
fnameplaystr=fnameplay((idx(end)+1):end);
idx=find(fnameplaystr=='_');
if(length(idx>0))
    for i1=1:length(idx)
        fnameplaystr(idx(i1))=' ';
    end
end


figNum=1;
scalea=5; scalew=5;
data=dataref;
%TimeSeq=double(data.time-BeatLen*data.BPB*(data.LeadIn-1))/BeatLen/data.BPB;
TimeSeq=double(data.time);
figure(figNum); clf; subplot(8,1,1)
plot(TimeSeq, data.wii1.ax, 'r');hold on
plot(TimeSeq, data.wii1.ay, 'g');
plot(TimeSeq, data.wii1.az, 'b');
axis([TimeSeq(1) TimeSeq(end) -scalea scalea]); title(['Reference, ', fnamerefstr]);
ylabel([Crefwii1,'a']); grid on;
%legend('x1', 'y1', 'z1');
subplot(8,1,3)
plot(TimeSeq, data.wii2.ax, 'r');hold on
plot(TimeSeq, data.wii2.ay, 'g');
plot(TimeSeq, data.wii2.az, 'b');
axis([TimeSeq(1) TimeSeq(end) -scalea scalea]);
ylabel([Crefwii2,'a']);grid on;
%legend('x2', 'y2', 'z2');
subplot(8,1,5)
plot(TimeSeq, data.wii1.yaw, 'r'); hold on
plot(TimeSeq, data.wii1.pitch, 'g');
plot(TimeSeq, data.wii1.roll, 'b');
axis([TimeSeq(1) TimeSeq(end) -scalew scalew]);
ylabel([Crefwii1, 'w']);grid on;
%legend('yaw2', 'pitch2', 'roll2');
subplot(8,1,7)
plot(TimeSeq, data.wii2.yaw, 'r'); hold on
plot(TimeSeq, data.wii2.pitch, 'g');
plot(TimeSeq, data.wii2.roll, 'b');
axis([TimeSeq(1) TimeSeq(end) -scalew scalew]);
ylabel([Crefwii2, 'w']);grid on;
%legend('yaw2', 'pitch2', 'roll2');

data=dataplay;

%TimeSeq=double(data.time-BeatLen*data.BPB*(data.LeadIn-1))/BeatLen/data.BPB;
TimeSeq=double(data.time);
subplot(8,1,2)
plot(TimeSeq, data.wii1.ax, 'r');hold on
plot(TimeSeq, data.wii1.ay, 'g');
plot(TimeSeq, data.wii1.az, 'b');
axis([TimeSeq(1) TimeSeq(end) -scalea scalea]); title(['Play, ', fnameplaystr]);
ylabel([Cplaywii1, 'a']);grid on;
%legend('x1', 'y1', 'z1');
subplot(8,1,4)
plot(TimeSeq, data.wii2.ax, 'r');hold on
plot(TimeSeq, data.wii2.ay, 'g');
plot(TimeSeq, data.wii2.az, 'b');
axis([TimeSeq(1) TimeSeq(end) -scalea scalea]);
ylabel([Cplaywii2, 'a']);grid on;
%legend('x2', 'y2', 'z2');
subplot(8,1,6)
plot(TimeSeq, data.wii1.yaw, 'r'); hold on
plot(TimeSeq, data.wii1.pitch, 'g');
plot(TimeSeq, data.wii1.roll, 'b');
axis([TimeSeq(1) TimeSeq(end) -scalew scalew]);
ylabel([Cplaywii1, 'w']);grid on;
%legend('yaw2', 'pitch2', 'roll2');
subplot(8,1,8)
plot(TimeSeq, data.wii2.yaw, 'r'); hold on
plot(TimeSeq, data.wii2.pitch, 'g');
plot(TimeSeq, data.wii2.roll, 'b');
axis([TimeSeq(1) TimeSeq(end) -scalew scalew]);
ylabel([Cplaywii2, 'w']);grid on;
%legend('yaw2', 'pitch2', 'roll2');


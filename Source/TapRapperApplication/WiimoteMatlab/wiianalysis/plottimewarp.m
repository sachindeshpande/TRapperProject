function plottimewarp(handles)
global WiiGUIData

DoNormal=1;
FAST=1;
playtype=WiiGUIData.ref{WiiGUIData.currentref};

switch playtype
    case 'feet'
        SubSample=16;
        PlayWindow=32;
        %starrange=[1, 2, 4, 6, 100];
        starrange=[3, 3.8, 4.5, 6., 100];
    case 'toe_stand'
        SubSample=64;
        PlayWindow=64;
        %starrange=[0.9, 1.4, 1.55, 1.85, 100];
        starrange=[1.1, 1.6, 1.75, 1.85, 100];
    case 'pop_over'
        SubSample=64;
        %starrange=[0.8, 1.35, 1.5, 1.8, 100];
        PlayWindow=64;
        starrange=[0.8, 1.45, 1.6, 1.8, 100]; %[5, 4, 3, 2, 1]
end
%PlayWindow=PlayWindow*20;
st=reshape([5 4 3 2 1; starrange], 1, 10);
set(handles.textThreshold, 'String', num2str(st));

dataref=WiiGUIData.dataref;
dataplay=WiiGUIData.dataplay;

if WiiGUIData.LRFlip
    twii=dataplay.wii1;
    dataplay.wii1=dataplay.wii2;
    dataplay.wii2=twii;
end

if DoNormal
    dataref=normalizewiidata(dataref);
    dataplay=normalizewiidata(dataplay);
end

BeatLen=60000/dataref.BPM; %in miliseconds

PlayBegin=max(dataref.time(1), dataplay.time(1));  %in miliseconds
PlayEnd=min(dataref.time(end), dataplay.time(end));  %in miliseconds
DurationRatio=(double(PlayEnd-PlayBegin))/double(dataref.playlength);
if DurationRatio<0.9
    msgbox('play is too short');
end

data1=[dataref.wii1.ax'; dataref.wii1.ay'; dataref.wii1.az'; ...
    dataref.wii1.pitch'; dataref.wii1.roll'; dataref.wii1.yaw'; ...
    dataref.wii2.ax'; dataref.wii2.ay'; dataref.wii2.az'; ...
    dataref.wii2.pitch'; dataref.wii2.roll'; dataref.wii2.yaw'];
data2=[dataplay.wii1.ax'; dataplay.wii1.ay'; dataplay.wii1.az'; ...
    dataplay.wii1.pitch'; dataplay.wii1.roll'; dataplay.wii1.yaw'; ...
    dataplay.wii2.ax'; dataplay.wii2.ay'; dataplay.wii2.az'; ...
    dataplay.wii2.pitch'; dataplay.wii2.roll'; dataplay.wii2.yaw'];
    
    
%(BeatLen/SubSample)
tsequence=double(PlayBegin):double(BeatLen/SubSample):double(PlayEnd);
beatsequence=(tsequence-4*BeatLen)/BeatLen/dataref.BPB;
    

t1=double(dataref.time);
%Method='linear';
%Method='spline';
Method='cubic';

%data1b=interp1(t1', data1', tsequence, Method);
data1b=interp1(t1', data1', tsequence);

t1=double(dataplay.time);
%data2b=interp1(t1', data2', tsequence, Method);
data2b=interp1(t1', data2', tsequence);

data1b=data1b'; data2b=data2b';
%data1b(:,3)=data1b(:,3)-1;
%data2b(:,3)=data2b(:,3)-1;


[score, p, q, c, sm]=getscoregrayscale(data1b, data2b, FAST, SubSample, dataref.NumBar, PlayWindow);
[scoreBW, PScore, Pmask, BWp, BWq, BWc, sm]=getscoreBW(data1b, data2b, FAST, PlayWindow);


score1=score;

stars=length(find(score<starrange));
    
addmatchline(handles, p, q, c, tsequence);

p1=beatsequence(p);
q1=beatsequence(q);

axes(handles.axesSM);
imagesc(beatsequence, beatsequence, sm);
colormap(1-gray);
hold on;
plot(q1, p1, 'r'); hold off
axes(handles.axesPath);
imagesc(beatsequence, beatsequence, c);
hold on; plot(q1,p1,'r'); grid on; hold off
title(['score: ', num2str(score), 'scoreBW: ', num2str(scoreBW), ' stars: ', num2str(stars)]);


avedelay=mean((q1-p1)*dataref.BPB);
fluctuation=std((q1-p1)*dataref.BPB);

%find reference points that are very quiet and exclude from scoring


axes(handles.axesPixelcost);
plot(tsequence, PScore); hold off
axis([0, tsequence(end), -0.1 1.1]);

axes(handles.axesAL);
plot(tsequence, PScore*5-2.5, 'm');
plot(tsequence, Pmask*2+7, 'k');

axes(handles.axesAR);
plot(tsequence, Pmask*2+7, 'k');

axes(handles.axesSeq);
plot(p1, (q1-p1)*dataref.BPB);

axis([p1(1), p1(end), -1, 1]);
grid on;  ylabel('beat'); xlabel('bar');
title(['Timing match, average delay: ',num2str(avedelay),...
    'beat, fluctuation', num2str(fluctuation)] );

%warp dataplay.time to reference
idx=1; i1=1; pshort(idx)=p(i1); qshort(idx)=q(i1);
for i1=2:length(p)
    if q(i1)>q(i1-1)
        idx=idx+1;
        pshort(idx)=p(i1);
        qshort(idx)=q(i1);
    end
end
%p is reference axis, q is play axis

WiiGUIData.warptime=interp1(tsequence(qshort), tsequence(pshort), double(dataplay.time),'linear', 'extrap');


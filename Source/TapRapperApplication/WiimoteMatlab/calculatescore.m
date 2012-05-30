function [score, stars, p, q]=calculatescore(dataref, dataplay, playinfo)
global Result

switch playinfo.PlayType
    case {'Feet', 'FEET'}
        SubSample=16;
        %Top=65; OneStar=20; %Top is slight better than what Wei can achieve, OneStar is slight better than stand still.
        Top=73; Floor=15;
    case {'ToeStand','TOESTAND'}
        SubSample=64; Floor= 5;
        %Top=60; OneStar=15;
        Top=63; Floor=5;
    case {'PopOver','POPOVERS'}
        SubSample=64;
        %Top=75; OneStar=15;
        Top=77; Floor=4;
end
%Delta=(Top-OneStar)/4;
%starrange=(1:5)*Delta; starrange(1)=-1;
starrange=(0:20:80)-0.00001; starrange(1)=-1000;
PlayWindow=64;
FAST=true;

dataref=normalizewiidata(dataref);
dataplay=normalizewiidata(dataplay);
BeatLen=60000/playinfo.BPM;

PlayBegin=max(dataref.time(1), dataplay.time(1));  %in miliseconds
PlayEnd=min(dataref.time(end), dataplay.time(end));  %in miliseconds
DurationRatio=(double(PlayEnd-PlayBegin))/double(dataref.playlength);
if DurationRatio<0.9
    disp('*********play is too short*******');
end

SwitchPlayLR=0;
if SwitchPlayLR
    wiit=dataplay.wii1;
    dataplay.wii1=dataplay.wii2;
    dataplay.wii2=wiit;
end

data1=[dataref.wii1.ax'; dataref.wii1.ay'; dataref.wii1.az'; ...
    dataref.wii1.pitch'; dataref.wii1.roll'; dataref.wii1.yaw'; ...
    dataref.wii2.ax'; dataref.wii2.ay'; dataref.wii2.az'; ...
    dataref.wii2.pitch'; dataref.wii2.roll'; dataref.wii2.yaw'];
data2=[dataplay.wii1.ax'; dataplay.wii1.ay'; dataplay.wii1.az'; ...
    dataplay.wii1.pitch'; dataplay.wii1.roll'; dataplay.wii1.yaw'; ...
    dataplay.wii2.ax'; dataplay.wii2.ay'; dataplay.wii2.az'; ...
    dataplay.wii2.pitch'; dataplay.wii2.roll'; dataplay.wii2.yaw'];

tsequence=double(PlayBegin):double(BeatLen/SubSample):double(PlayEnd);
t1=double(dataref.time);
Method='cubic';
data1b=interp1(t1', data1', tsequence, Method);

t1=double(dataplay.time);
data2b=interp1(t1', data2', tsequence, Method);
data1b=data1b'; data2b=data2b';
[score, pscore, Pmask, p, q, c, sm]=getscoreBW(data1b, data2b, FAST, PlayWindow);
score_org=score;
score=round((score-Floor)*100/(Top-Floor));
if score > 100
    score = 100;
end
stars=length(find(score>starrange));

Result.score=score;
Result.pscore=pscore;
Result.p=p;
Result.q=q;
Result.tsequence=tsequence;
Result.score_org=score_org;
Result.Top=Top;
Result.Floor=Floor;


%disp([score, stars]);

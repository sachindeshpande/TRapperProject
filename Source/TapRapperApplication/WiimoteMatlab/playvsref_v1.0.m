function playvsref(playtype, fnameplay)
global stars score ScoreEnv

%initialization
tpath=pwd;
idx=strfind(tpath, '\');
rootpath=tpath(1:idx(end));
%refpath='D:\Projects\WiimoteProjects\WiiMotionPlus\Version29-WithTUIO\Data\matlab\';
refpath=[rootpath, 'Data\matlab\'];
matlabpath=[rootpath, 'WiimoteMatlab\'];
addpath(matlabpath);

DoCal = 1;
DoPlot = 1;
DoNormal=1;
SubSample=64;
FAST=1;

feetidx=strfind(fnameplay, 'Feet');
if length(feetidx)>0
    playtype='FEET';
end

[reference, starrange, SubSample, PlayWindow]=setScoreEnv(playtype, refpath);


load(reference);
dataplay=readwiidata(fnameplay);

%check wiiplay data
if isDataOK(dataplay)<1
    return;
end


Crefwii1='L'; Crefwii2='R';
Cplaywii1='L'; Cplaywii2='R';

if DoCal
    dataplay=applyrobustcalibration(dataplay);
    %dataplay=applycalibration(dataplay);
end

refend=dataref.time(end);
playend=dataplay.time(end);
offset=refend-playend;

dataref=cutdata(dataref, dataref.refstart, dataref.playlength);
dataplay=cutdata(dataplay, dataref.playstart-offset, dataref.playlength);

if DoNormal
    dataref=normalizewiidata(dataref);
    dataplay=normalizewiidata(dataplay);
end

BeatLen=60000/dataref.BPM; %in miliseconds

if DoPlot
    plotplayvsref
end

PlayBegin=max(dataref.time(1), dataplay.time(1));  %in miliseconds
PlayEnd=min(dataref.time(end), dataplay.time(end));  %in miliseconds
DurationRatio=(double(PlayEnd-PlayBegin))/double(dataref.playlength);
if DurationRatio<0.9
    disp('*********play is too short*******');
end

score=100; scoreBWreport=0;
for SwitchPlayLR=0:1
    %switch play data LR for testing purpose
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
    
    
    %(BeatLen/SubSample)
    tsequence=double(PlayBegin):double(BeatLen/SubSample):double(PlayEnd);
    beatsequence=(tsequence-4*BeatLen)/BeatLen/dataref.BPB;
    
    
    t1=double(dataref.time);
    %Method='linear';
    %Method='spline';
    Method='cubic';
    
    data1b=interp1(t1', data1', tsequence, Method);
    
    t1=double(dataplay.time);
    data2b=interp1(t1', data2', tsequence, Method);
 
    data1b=data1b'; data2b=data2b'; 
    %[tscore0, p, q, c, sm]=getscoregrayscale(data1b, data2b, FAST, SubSample, dataref.NumBar, PlayWindow);
    [scoreBW, PScore, Pmask, BWp, BWq, BWc, sm]=getscoreBW(data1b, data2b, FAST, PlayWindow);
    [tscore, p, q, c, sm]=getscoreOrg(data1b, data2b, FAST, dataref.NumBar, SubSample, DurationRatio);
    disp([tscore0, tscore, scoreBW]);
    if SwitchPlayLR==1
        if tscore<score
            disp(['******LR reversed in data******']);
        end
    end
    
    if tscore<score
        PlotAgain=true;
    end
    score = min(score, tscore);
    stars=length(find(score<starrange));

    scoreBWreport=max(scoreBW, scoreBWreport);
    
    
    p1=beatsequence(p);
    q1=beatsequence(q);
    if (DoPlot & PlotAgain)
        figure(1); subplot(3,3,3);
        imagesc(beatsequence, beatsequence, sm);
        colormap(1-gray);
        hold on;
        plot(q1, p1, 'r'); hold off
        subplot(336);
        imagesc(beatsequence, beatsequence, c);
        hold on; plot(q1,p1,'r'); grid on; hold off
        title(['score: ', num2str(score), ' stars: ', num2str(stars)]);
        subplot(3,1,3);
        plot(p1, (q1-p1)*dataref.BPB);
        
        axis([p1(1), p1(end), -1, 1]);
        grid on;  ylabel('beat'); xlabel('bar');
        
        PlotAgain=false;
%         title(['Timing match, average delay: ',num2str(avedelay),...
%             'beat, fluctuation', num2str(fluctuation),...
%             ' Cal', num2str(DoCal), ' Normaled', num2str(DoNormal)] );
    end
    
    
%     avedelay=mean((q1-p1)*dataref.BPB);
%     fluctuation=std((q1-p1)*dataref.BPB);
    
    
end

msgbox(['new score is: ', num2str(round(scoreBWreport))]);
disp(['score: ', num2str(score), ' stars: ', num2str(stars)]);

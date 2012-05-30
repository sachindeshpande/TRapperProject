function playvsref(playtype, fnameplay)
global stars score

refpath='D:\Projects\WiimoteProjects\WiiMotionPlus\Version29-WithTUIO\Data\matlab\';

DoCal = 1;
DoPlot = 1;
DoNormal=1;
SubSample=64;
FAST=1;

switch playtype
    case 'FEET'
        SubSample=16;
        reference=[refpath, 'feet.mat'];
        %starrange=[1, 2, 4, 6, 100];
        %starrange=[2.3, 2.8, 3.3, 3.53, 100];
        starrange=[3, 3.8, 4.5, 6., 100];
    case 'TOESTAND'
        SubSample=64;
        reference=[refpath, 'toe_stand.mat'];
        %starrange=[0.9, 1.4, 1.55, 1.85, 100];
        starrange=[1.1, 1.6, 1.75, 1.85, 100];
    case 'POPOVERS'
        SubSample=64;
        reference=[refpath, 'pop_over.mat'];
        %starrange=[0.8, 1.35, 1.5, 1.8, 100];
        %starrange=[0.8, 1.15, 1.25, 1.8, 100]; %[5, 4, 3, 2, 1]
        starrange=[0.8, 1.45, 1.6, 1.8, 100]; %[5, 4, 3, 2, 1]
end

load(reference);
dataplay=readwiidata(fnameplay);

%check wiiplay data
if length(dataplay.time)<100
    msgbox(['play data very short, length is ', num2str(length(dataplay.time)), ', please check wiimote connection.']);
end

test=dataplay.wii1.ax;
test=test-test(1);
if sum(abs(test))<0.0001
    msgbox('Left wiimote Acc droped connection!');
end

test=dataplay.wii2.ax;
test=test-test(1);
if sum(abs(test))<0.0001
    msgbox('Right wiimote Acc droped connection!');
end

test=dataplay.wii1.pitch;
test=test-test(1);
if sum(abs(test))<0.0001
    msgbox('Left wiimote Gyro droped connection!');
end

test=dataplay.wii2.pitch;
test=test-test(1);
if sum(abs(test))<0.0001
    msgbox('Right wiimote Gyro droped connection!');
end
%end dataplay test


Crefwii1='L'; Crefwii2='R';
Cplaywii1='L'; Cplaywii2='R';

if DoCal
    dataplay=applyrobustcalibration(dataplay);
    %dataplay=applycalibration(dataplay);
end

dataref=cutdata(dataref, dataref.refstart, dataref.playlength);
dataplay=cutdata(dataplay, dataref.playstart+0, dataref.playlength);

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

score=100;
for SwitchPlayLR=1
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
    sm=simmx(data1b, data2b);
    
    if FAST
        [p, q, c]=dpfast(1-sm);
    else
        [p, q, c]=dp2(1-sm);
    end
    
    tscore=c(size(c,1), size(c,2))/dataref.NumBar/SubSample/mean(mean(data1b))/DurationRatio;
    if SwitchPlayLR==1
        if tscore<score
            disp(['******LR reversed in data******']);
        end
    end
    score = min(score, tscore);
    stars=length(find(score<starrange));
    
    
    p1=beatsequence(p);
    q1=beatsequence(q);
    if DoPlot
        subplot(3,3,3);
        imagesc(beatsequence, beatsequence, sm);
        colormap(1-gray);
        hold on;
        plot(q1, p1, 'r'); hold off
        subplot(336);
        imagesc(beatsequence, beatsequence, c);
        hold on; plot(q1,p1,'r'); grid on; hold off
        title(['score: ', num2str(score), ' stars: ', num2str(stars)]);
    end
    
    avedelay=mean((q1-p1)*dataref.BPB);
    fluctuation=std((q1-p1)*dataref.BPB);
    
    if DoPlot
        subplot(3,1,3);
        plot(p1, (q1-p1)*dataref.BPB);
        
        axis([p1(1), p1(end), -1, 1]);
        grid on;  ylabel('beat'); xlabel('bar');
        title(['Timing match, average delay: ',num2str(avedelay),...
            'beat, fluctuation', num2str(fluctuation),...
            ' Cal', num2str(DoCal), ' Normaled', num2str(DoNormal)] );
        if SwitchPlayLR<1
           % pause 1
        end
    end
    
end

disp(['score: ', num2str(score), ' stars: ', num2str(stars)]);

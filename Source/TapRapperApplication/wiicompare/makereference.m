%function playvsref(fnameref, fnameplay, DoCal, DoNormal)

%fnameref='D:\Projects\WiimoteProjects\WiiMotionPlus\Version26-WithTUIO\Data\WiimoteReferenceData\Reference6_Try_2.csv';
%calfile='D:\Projects\WiimoteProjects\WiiMotionPlus\Version26-WithTUIO\Data\WiimoteReferenceData\data2010May10\StandStill.csv';

%DeltaT=5; %this should be a global variable
if not(exist('fpath'))
    fpath=pwd;
end
[fnameref, fpath]=uigetfile('*.csv', 'Open Reference', [fpath, '/a.csv']); %ref6_try4
[fnameplay, fpathplay]=uigetfile('*.csv', 'Open play', [fpath, '/a.csv']);

fnameref=[fpath, fnameref];
fnameplay=[fpathplay, fnameplay];

dataref=readwiidata(fnameref);
dataplay=readwiidata(fnameplay);

idx=find(fnameref=='\');
fnamerefstr=fnameref((idx(end)+1):end-4);
fpath=fnameref(1:idx(end))
% 
% Crefwii1='R'; Crefwii2='L';

% dataref=applycalibration(dataref)
% dataplay=applycalibration(dataplay)
dataref=applyrobustcalibration(dataref)
dataplay=applyrobustcalibration(dataplay)

%checkcalibration;


%modifying the range
tstart=0; tend=dataref.time(end);
tlength=tend-tstart;
%tstart=4400; tend=13800; %toestand
%tstart=13500; tend=23000; %popover
fignum=1; NumBar=4; BPB=4; PlotGyro=1; Subplot=0;
clf; plotwiidata(fignum, dataref, tstart, tlength, NumBar, BPB, PlotGyro, Subplot);

fignum=1; NumBar=4; BPB=4; PlotGyro=0; Subplot=0;
clf; plotwiidata(fignum, dataref, tstart, tlength, NumBar, BPB, PlotGyro, Subplot);
title('reference');
fignum=1; NumBar=4; BPB=4; PlotGyro=0; Subplot=2;
plotwiidata(fignum, dataplay, tstart, tlength, NumBar, BPB, PlotGyro, Subplot);
title('play');


ADJUST = true

while ADJUST
    [x, y]=ginput(2);
    tstart=max(0, x(1));
    tend=min(dataref.time(end), x(2)); 
    tlength=tend-tstart;
%     fignum=1; NumBar=4; BPB=4; PlotGyro=1; Subplot=0;
%     clf; plotwiidata(fignum, dataref, tstart, tlength, NumBar, BPB, PlotGyro, Subplot);
    fignum=1; NumBar=4; BPB=4; PlotGyro=0; Subplot=0;
    clf; plotwiidata(fignum, dataref, tstart, tlength, NumBar, BPB, PlotGyro, Subplot);
    title('reference');
    fignum=1; NumBar=4; BPB=4; PlotGyro=0; Subplot=2;
    plotwiidata(fignum, dataplay, tstart, tlength, NumBar, BPB, PlotGyro, Subplot);
    title('play');
    
    keyin=input('more adjust, 0 to quit')
    if length(keyin)>0
        if keyin==0
            ADJUST=false;
        end
    end
end    
    

dataref.playstart=tstart;
dataref.playlength=tlength;
dataref.refstart=tstart;
    
dataref.originfname=fnameref;
%dataref.fname='toestand';
%dataref.fname='popover';
dataref.fname='feet';

%save([fpath, dataref.fname], 'dataref') 

function playvsref(playtype, fnameplay)
global stars score ViewAxes ViewHandle Result Log

%initialization
tpath=pwd;
idx=strfind(tpath, '\');
rootpath=tpath(1:idx(end));
refpath=[rootpath, 'Data\matlab\'];
matlabpath=[rootpath, 'WiimoteMatlab\'];
addpath(matlabpath);

Log.path=[rootpath, 'Data\'];
Log.fname=[Log.path, 'taprappermatlablog.txt'];
Log.DoLog=true;

DoPlot = 1;
FAST=1;
Alignment='Align End';
%Alignment='Align Begining';


feetidx=strfind(fnameplay, 'Feet');
if length(feetidx)>0
    playtype='FEET';
end

switch playtype
    case 'FEET'
        reference=[refpath, 'ref_feet_june29_play1.mat'];
        str_filter='feet';
    case 'TOESTAND'
        reference=[refpath, 'ref_toestand_june29_play2.mat'];
        str_filter='toe';
    case 'POPOVERS'
        reference=[refpath, 'ref_popover_june29_play3.mat'];
        str_filter='pop';
end
tic;
load(reference);

dataplay=readwiidata(fnameplay);
infofname=dir([refpath, 'info*',str_filter, '*.mat']);
load([refpath, infofname(1).name]);
playinfo.PlayType=playtype;

if Log.DoLog
    fid=fopen(Log.fname, 'a');
    DateStr=datestr(now);
    fprintf(fid, '\n%s\n%s\n%s\n', DateStr, reference, fnameplay);
    fclose(fid);
end

t(1)=toc; tic;

%check wiiplay data
if isDataOK(dataplay)<1
    return;
end

dataplay=applyrobustcalibration(dataplay);

offset=0;
switch Alignment
    case 'Align End'
        refend=dataref.time(end);
        playend=dataplay.time(end);
        offset=refend-playend;
    case 'Align Begining'
        offset=0;
end

dataref=cutdata(dataref, playinfo.PlayStart, playinfo.PlayLength);
dataplay=cutdata(dataplay, playinfo.PlayStart-offset, playinfo.PlayLength);
t(2)=toc; tic;

[score, stars, p, q]=calculatescore(dataref, dataplay, playinfo);
the_result=Result;
t(3)=toc; tic;


if stars<3
    %try switch LR
    wiit=dataplay.wii1;
    dataplay.wii1=dataplay.wii2;
    dataplay.wii2=wiit;
    [scoreX, starsX, pX, qX]=calculatescore(dataref, dataplay, playinfo);
    if scoreX>score
        log_warning=['!!!!!!LR might be reversed in data log. Old score:', num2str(round(score)),...
            'after flip LR:', num2str(round(scoreX))];
        disp(log_warning);
        score=scoreX; stars=starsX; p=pX; q=qX;
        the_result=Result;
        if Log.DoLog
            fid=fopen(Log.fname, 'a');
            fprintf(fid, '%s\n', log_warning);
            fclose(fid);
        end
    end
end
t(4)=toc; tic;

report_txt=['score:', num2str(round(score)), ' stars:', num2str(stars)];
report_txt={report_txt; [num2str(the_result.Floor), ' ',...
    num2str(round(the_result.score_org)),' ', num2str(the_result.Top)]};
if Log.DoLog
    fid=fopen(Log.fname, 'a');
    fprintf(fid, '%s\n', [report_txt{1}, ' ', report_txt{2}]);
    fclose(fid);
end

if DoPlot
    InitPlot=1;
    if exist('ViewAxes')
        if ishandle(ViewAxes')
            InitPlot=0;
        end
    end
    
    if InitPlot
        scrsz = get(0,'ScreenSize');
        figure('Position',[scrsz(3)*3/4 30 scrsz(3)/4 scrsz(4)/4]);
        ViewAxes=axes;
        ViewHandle=WiiView_Init(ViewAxes);
    end
    WiiView_plotPlay(ViewHandle.href, dataref, 10);
    WiiView_plotPlay(ViewHandle.hplay, dataplay, -10);
    WiiView_setXScale(ViewAxes, 0, playinfo.PlayLength);
    set(ViewAxes, 'Title', text('String', report_txt));
end
%msgbox(['score:', num2str(round(score)), ' stars:', num2str(stars)]);
 

t(5)=toc; 
disp(t);

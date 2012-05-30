function varargout = playanalysis(varargin)
gui_Singleton = 1;
gui_State = struct('gui_Name',       mfilename, ...
                   'gui_Singleton',  gui_Singleton, ...
                   'gui_OpeningFcn', @playanalysis_OpeningFcn, ...
                   'gui_OutputFcn',  @playanalysis_OutputFcn, ...
                   'gui_LayoutFcn',  [] , ...
                   'gui_Callback',   []);
if nargin && ischar(varargin{1})
    gui_State.gui_Callback = str2func(varargin{1});
end

if nargout
    [varargout{1:nargout}] = gui_mainfcn(gui_State, varargin{:});
else
    gui_mainfcn(gui_State, varargin{:});
end

function playanalysis_OpeningFcn(hObject, eventdata, handles, varargin)
global AnalysisData Log
handles.output = hObject;

guidata(hObject, handles);


Log.DoLog=false;
AnalysisData=[];
tpath=pwd;
plst=find(tpath=='\');
AnalysisData.rootpath=tpath(1:plst(end-1));
AnalysisData.datapath=['D:\Projects\WiimoteProjects\data\'];
set(handles.editDataDir, 'string', AnalysisData.datapath);
AnalysisData.infopath=[AnalysisData.rootpath, 'Data\matlab\'];
AnalysisData.LRFlip=0;
addpath([AnalysisData.rootpath, 'WiimoteMatlab']);
AnalysisData.Handles=handles;
AnalysisData.RefList=[];
AnalysisData.Play=[];
AnalysisData.StepCollection=[];
set(handles.checkboxShowAcc, 'value', 1);
set(handles.checkboxShowGyro, 'value', 1);
AnalysisData.Align=getPopupMenuString(handles.popupmenuAlign);

initReference();
AnalysisData.ViewHandle=WiiView_Init(handles.axesWiiView);
for i1=1:20
    set(AnalysisData.ViewHandle.hstep(i1).hwarp, 'ButtonDownFcn', @steplinereport);
end


function varargout = playanalysis_OutputFcn(hObject, eventdata, handles) 
varargout{1} = handles.output;

function popupmenuPlayType_Callback(hObject, eventdata, handles)
initReference();
setDataInfo(handles);

function popupmenuPlayType_CreateFcn(hObject, eventdata, handles)
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end

function listboxReference_Callback(hObject, eventdata, handles)
global AnalysisData
val=get(hObject, 'value');
strlst=get(hObject, 'string');
fname=strlst{val(1)};
AnalysisData.ref=loadReference([AnalysisData.infopath, fname]);
updateView();
listboxPlayList_Callback(handles.listboxPlayList, [], handles)

function ref=loadReference(fname)
global AnalysisData
load(fname);
dataref.OriginalLength=dataref.time(end);
ref=cutdata(dataref, AnalysisData.playinfo.PlayStart, AnalysisData.playinfo.PlayLength);

function listboxReference_CreateFcn(hObject, eventdata, handles)
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end

function pushbuttonSelectDataDir_Callback(hObject, eventdata, handles)
global AnalysisData
datapath=uigetdir(AnalysisData.datapath, 'Pick a play data directory');
if length(datapath)>1
    AnalysisData.datapath=datapath;
    setDataInfo(handles);
end

function editDataDir_Callback(hObject, eventdata, handles)

function editDataDir_CreateFcn(hObject, eventdata, handles)
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end

function listboxPlayList_Callback(hObject, eventdata, handles)
global AnalysisData Result
val=get(hObject, 'value');
strlst=get(hObject, 'string');
fname=[];
if length(strlst)>0
    fname=strlst{val(1)};
    if length(fname)>2
        AnalysisData.play=loadPlay([AnalysisData.datapath, '\',fname]);
        
        [score, stars, p, q]=calculatescore(AnalysisData.ref, AnalysisData.play, AnalysisData.playinfo);
        
        AnalysisData.Result=Result;
        
        strlist=get(handles.editViewInfo, 'string');
        output=['score:', num2str(round(score)), ' stars:', num2str(stars), '    ',...
            num2str(round([Result.Floor, Result.score_org, Result.Top]))];
        strlist=[{output}; strlist];
        set(handles.editViewInfo, 'string', strlist);
        updateView();
    end
end

function play=loadPlay(fname)
global AnalysisData

playdata=readwiidata(fname);
playdata.OrginalLength=playdata.time(end);
playdata=applyrobustcalibration(playdata);
Alignment=getPopupMenuString(AnalysisData.Handles.popupmenuAlign);

offset=0;
switch Alignment
    case 'Align End'
        offset=AnalysisData.ref.OriginalLength-playdata.OrginalLength;
    case 'Align Begining'
        offset=0;
end
play=cutdata(playdata, AnalysisData.playinfo.PlayStart-offset, AnalysisData.playinfo.PlayLength);
if get(AnalysisData.Handles.checkboxFlipLR, 'value') %need to switch LR
    wiit=play.wii1;
    play.wii1=play.wii2;
    play.wii2=wiit;
end


function listboxPlayList_CreateFcn(hObject, eventdata, handles)
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end

function pushbuttonRange_Callback(hObject, eventdata, handles)
axes(handles.axesWiiView);
ax=axis();
[x, y]=ginput(2);
axis([min(x), max(x), ax(3), ax(4)]);

function pushbuttonZoomOut_Callback(hObject, eventdata, handles)
axes(handles.axesWiiView);
ax=axis();
x1=ax(1); x2=ax(2);
xc=(x1+x2)/2;
halfwidth=(x2-x1)/2;
x1=xc-halfwidth*2;
x2=xc+halfwidth*2;
axis([x1, x2, ax(3), ax(4)]);

function pushbuttonMeasure_Callback(hObject, eventdata, handles)
global AnalysisData
[x, y]=ginput(2);
diff=x(2)-x(1);
diff_in_beat=round(diff/AnalysisData.playinfo.Beat*10)/10;
txt=['time space: ', num2str(round(diff)), 'ms, ', num2str(diff_in_beat), 'beat'];
msgbox(txt);


function checkboxFlipLR_Callback(hObject, eventdata, handles)
listboxPlayList_Callback(handles.listboxPlayList, [], handles)

function checkboxTimeWarp_Callback(hObject, eventdata, handles)

function popupmenuWarpDistance_Callback(hObject, eventdata, handles)
global AnalysisData
WiiView_clearWarpLines(AnalysisData.ViewHandle.hwarp);
updateView();

function popupmenuWarpDistance_CreateFcn(hObject, eventdata, handles)
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end

function checkboxShowWarp_Callback(hObject, eventdata, handles)
updateView();


function checkboxShowAcc_Callback(hObject, eventdata, handles)
updateView();

function checkboxShowGyro_Callback(hObject, eventdata, handles)
updateView();

function checkShowBarMark_Callback(hObject, eventdata, handles)
updateView();

function checkboxShowStepText_Callback(hObject, eventdata, handles)

function popupmenuAlign_Callback(hObject, eventdata, handles)
listboxPlayList_Callback(handles.listboxPlayList, [], handles)

function popupmenuAlign_CreateFcn(hObject, eventdata, handles)
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end


function string=getPopupMenuString(TheHandle)
idx=get(TheHandle, 'value');
strlist=get(TheHandle, 'String');
string=strlist{idx};

function initReference()
global AnalysisData
AnalysisData.PlayType=getPopupMenuString(AnalysisData.Handles.popupmenuPlayType);

switch AnalysisData.PlayType
    case 'Feet'
        AnalysisData.PlayFilter='feet';
    case 'ToeStand'
        AnalysisData.PlayFilter='toe';
    case 'PopOver'
        AnalysisData.PlayFilter='pop';
end

flist=dir([AnalysisData.infopath, 'ref*',AnalysisData.PlayFilter, '*.mat']);

AnalysisData.ReferenceList=[];
for i1=1:length(flist)
    AnalysisData.ReferenceList{i1}=flist(i1).name;
end

set(AnalysisData.Handles.listboxReference, 'Max', length(flist));
set(AnalysisData.Handles.listboxReference, 'String', AnalysisData.ReferenceList);
set(AnalysisData.Handles.listboxReference, 'Val', 1:length(flist));

infofname=dir([AnalysisData.infopath, 'info*',AnalysisData.PlayFilter, '*.mat']);
load([AnalysisData.infopath, infofname(1).name]);
playinfo.PlayType=AnalysisData.PlayType;
AnalysisData.playinfo=playinfo;

function setDataInfo(handles)
global AnalysisData
set(handles.editDataDir, 'String', AnalysisData.datapath);
flist=dir([AnalysisData.datapath, '\*', AnalysisData.PlayFilter, '*.csv']);
AnalysisData.dataselected=0;

if(length(flist)<1)
    AnalysisData.hasdata=false;
else
    AnalysisData.hasdata=true;
end

datafilelist{1}=' ';
for i1=1:length(flist)
    datafilelist{i1}=flist(i1).name;
end
AnalysisData.datafilelist=datafilelist;

set(handles.listboxPlayList, 'Val', 1);
set(handles.listboxPlayList, 'String', datafilelist);

function updateView()
global AnalysisData
H=AnalysisData.Handles;

if isfield(AnalysisData, 'ref')
    WiiView_plotPlay(AnalysisData.ViewHandle.href, AnalysisData.ref, 10);
    %xmin=AnalysisData.playinfo.PlayStart;
    xmin=0;
    xmax=xmin+AnalysisData.playinfo.PlayLength;
    WiiView_setXScale(H.axesWiiView, xmin, xmax);
    WiiView_plotBarMark(AnalysisData.ViewHandle.hbar, AnalysisData.playinfo.NumBar,...
        AnalysisData.playinfo.BPB, AnalysisData.playinfo.Beat)
end
if isfield(AnalysisData, 'play')
    WiiView_plotPlay(AnalysisData.ViewHandle.hplay, AnalysisData.play, -10);
end


ShowAcc=get(H.checkboxShowAcc, 'value');
ShowGyro=get(H.checkboxShowGyro, 'value');
ShowBarMark=get(H.checkShowBarMark, 'value');
ShowWarpLine=get(H.checkboxShowWarp, 'value');
if not(ShowAcc)
    WiiView_clearPlot(AnalysisData.ViewHandle.href, 'Acc');
    WiiView_clearPlot(AnalysisData.ViewHandle.hplay, 'Acc');
end
if not(ShowGyro)
    WiiView_clearPlot(AnalysisData.ViewHandle.href, 'Gyro');
    WiiView_clearPlot(AnalysisData.ViewHandle.hplay, 'Gyro');
end
if not(ShowBarMark)
    WiiView_clearBarMark(AnalysisData.ViewHandle.hbar);
end

if not(ShowWarpLine)
    WiiView_clearWarpLines(AnalysisData.ViewHandle.hwarp);
else
    if isfield(AnalysisData, 'Result')
        Space=str2num(getPopupMenuString(H.popupmenuWarpDistance));
        WiiView_plotWarpLines(AnalysisData.ViewHandle.hwarp, AnalysisData.playinfo.PlayLength,...
            AnalysisData.playinfo.Beat, Space, AnalysisData.Result.p, AnalysisData.Result.q);
    end
end

if length(AnalysisData.StepCollection)>0
    StepCollection=AnalysisData.StepCollection;
    analyzeSteps;
    WiiView_clearStep(AnalysisData.ViewHandle.hstep);
    WiiView_plotStep(AnalysisData.ViewHandle.hstep, AnalysisData.StepCollection.step);
end


function editViewInfo_Callback(hObject, eventdata, handles)
function editViewInfo_CreateFcn(hObject, eventdata, handles)
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end

function pushbuttonAddAStep_Callback(hObject, eventdata, handles)
global AnalysisData
axes(handles.axesWiiView);
[x, y]=ginput(2);

step.center=mean(x);
step.width=max(x)-min(x);
step.left=min(x); step.right=max(x);

step=analyzeAStep(step);

reportTxt={['Partial Score:', num2str(round(step.score)), ' timing offset:', num2str(step.offset), 'ms, ',...
    num2str(step.offset_in_beat), 'Beat']; 'Add to Step Collection?'};
UserSelection=questdlg(reportTxt, 'Add to Step Collection?', 'Yes');
if strcmp(UserSelection, 'Yes')
    addToStepCollection(step);
end
    
WiiView_plotStep(AnalysisData.ViewHandle.hstep, AnalysisData.StepCollection.step);

function analyzeSteps()
global AnalysisData

if isfield(AnalysisData, 'Result')
    for i1=1:length(AnalysisData.StepCollection.step)
        AnalysisData.StepCollection.step(i1)=analyzeAStep(AnalysisData.StepCollection.step(i1));
    end
end

function step=analyzeAStep(step)
global AnalysisData    

stepL=step.left;
stepR=step.right;

tseq=AnalysisData.Result.tsequence;
idx=find((tseq>stepL)&(tseq<stepR));
StepScore=(length(idx)-sum(AnalysisData.Result.pscore(idx)))/length(idx)*100;
    
tp=tseq(AnalysisData.Result.p);
tq=tseq(AnalysisData.Result.q);
idx=find((tp>stepL)&(tp<stepR));
tp_center=round(mean(tp(idx)));
tq_center=round(mean(tq(idx)));
offset=tq_center-tp_center;
offset_in_beat=offset/AnalysisData.playinfo.Beat;
offset_in_beat=0.1* round(offset_in_beat*10);

step.qleft=tq(idx(1)); step.qright=tq(idx(end));
step.offset=offset;
step.offset_in_beat=offset_in_beat;
step.score=StepScore;

function addToStepCollection(step0)
global AnalysisData
if isempty(AnalysisData.StepCollection)
    AnalysisData.StepCollection.step(1)=step0;
else
    if (step0.center<AnalysisData.StepCollection.step(1).center)
        AnalysisData.StepCollection.step(2:end+1)=AnalysisData.StepCollection.step;
        AnalysisData.StepCollection.step(1)=step0;
    elseif (step0.center>AnalysisData.StepCollection.step(end).center)
        AnalysisData.StepCollection.step(end+1)=step0;
    else
        numsteps=length(AnalysisData.StepCollection.step);
        i1=1;
        while (i1<=numsteps)& (AnalysisData.StepCollection.step(i1).center<step0.center)
            i1=i1+1;
        end
        AnalysisData.StepCollection.step((i1+1):(end+1))=AnalysisData.StepCollection.step(i1:end);
        AnalysisData.StepCollection.step(i1)=step0;
    end
end

function pushbuttonRemoveAStep_Callback(hObject, eventdata, handles)
global AnalysisData
axes(handles.axesWiiView);
[x, y]=ginput(1);
iclear=0;
numsteps=length(AnalysisData.StepCollection.step);
for i1=1:numsteps
    stepL=AnalysisData.StepCollection.step(i1).left;
    stepR=AnalysisData.StepCollection.step(i1).right;
    if (stepR>x)&(x>stepL)
        iclear=i1;
    end
end
if iclear>0
    if numsteps==1
        AnalysisData.StepCollection=[];
    else
        if iclear==numsteps
            newsteps=AnalysisData.StepCollection.step(1:(end-1));
        elseif iclear ==1
            newsteps=AnalysisData.StepCollection.step(2:end);
        else
            newsteps=AnalysisData.StepCollection.step(1:iclear-1);
            newsteps(iclear:numsteps-1)=AnalysisData.StepCollection.step(iclear+1:end);
        end
        AnalysisData.StepCollection.step=newsteps;
    end
    WiiView_clearStep(AnalysisData.ViewHandle.hstep);
    WiiView_plotStep(AnalysisData.ViewHandle.hstep, AnalysisData.StepCollection.step);
end
    

function pushbuttonLoadSteps_Callback(hObject, eventdata, handles)
global AnalysisData
StepCollection=AnalysisData.StepCollection;

[fname, pathname]=uigetfile([AnalysisData.infopath, '\steps*', AnalysisData.PlayFilter, '*.mat']);
if fname
    load([pathname, fname]);
    AnalysisData.StepCollection=StepCollection;
    analyzeSteps;
    WiiView_clearStep(AnalysisData.ViewHandle.hstep);
    WiiView_plotStep(AnalysisData.ViewHandle.hstep, AnalysisData.StepCollection.step);
end

% --- Executes on button press in pushbuttonClearSteps.
function pushbuttonClearSteps_Callback(hObject, eventdata, handles)
global AnalysisData
AnalysisData.StepCollection=[];
WiiView_clearStep(AnalysisData.ViewHandle.hstep);

% --- Executes on button press in pushbuttonSaveSteps.
function pushbuttonSaveSteps_Callback(hObject, eventdata, handles)
global AnalysisData
StepCollection=AnalysisData.StepCollection;

[fname, pathname]=uiputfile([AnalysisData.infopath, '\steps*', AnalysisData.PlayFilter, '*.mat']);
if fname
    save([pathname, fname], 'StepCollection');
end

function steplinereport(src,evnt)
% src - the object that is the source of the event
% evnt - empty for this property
% sel_typ = get(gcbf,'SelectionType')
% switch sel_typ
%     case 'normal'
%         disp('User clicked left-mouse button')
%         set(src,'Selected','on')
%     case 'extend'
%         disp('User did a shift-click')
%         set(src,'Selected','on')
%     case 'alt'
%         disp('User did a control-click')
%         set(src,'Selected','on')
%         set(src,'SelectionHighlight','off')
% end
x=get(src, 'xdata');
spyAStep(x(1));

% --- Executes on button press in pushbuttonSpyAStep.
function pushbuttonSpyAStep_Callback(hObject, eventdata, handles)
global AnalysisData
axes(handles.axesWiiView);
[x, y]=ginput(1);
spyAStep(x);

function spyAStep(x);
global AnalysisData

ispy=0;
numsteps=length(AnalysisData.StepCollection.step);
for i1=1:numsteps
    stepL=AnalysisData.StepCollection.step(i1).left;
    stepR=AnalysisData.StepCollection.step(i1).right;
    if (stepR>x)&(x>stepL)
        ispy=i1;
    end
end
if not(ispy==0)
    step=AnalysisData.StepCollection.step(ispy);
    reportTxt=['at ', num2str(round(x/100)/10), 's, partial Score:', num2str(round(step.score)), ' timing offset:', num2str(step.offset), 'ms, ',...
    num2str(step.offset_in_beat), 'Beat'];
	%msgbox(reportTxt);
    strlist=get(AnalysisData.Handles.editViewInfo, 'string');
    
    strlist=[{reportTxt}; strlist];
    set(AnalysisData.Handles.editViewInfo, 'string', strlist);

end

function pushbuttonReportSteps_Callback(hObject, eventdata, handles)
global AnalysisData

numsteps=length(AnalysisData.StepCollection.step);
timing=[];
igood=1;
for i1=1:numsteps
    step=AnalysisData.StepCollection.step(i1);
    if step.score>40;
        timing(igood)=step.offset_in_beat;
        igood=igood+1;
    end
end
if igood<3
    reportTxt='Too few steps are accepted';
else
    drift=mean(timing);
    wobble=std(timing);
    reportTxt={['steps accepted:', num2str(igood-1)],...
        ['drift:', num2str(round(drift*100)/100), 'Beat, wobble:', num2str(round(wobble*100)/100), 'Beat']};
    if (drift<0.2)&(wobble<0.2)
        reportTxt{end+1}='great timing';
    else
        if drift>0.2
            reportTxt{end+1}='Watch the Beat';
        end
        if wobble > 0.2
            reportTxt{end+1}='Keep even Beat';
        end
    end
end
strlist=get(AnalysisData.Handles.editViewInfo, 'string');

strlist=[' '; reportTxt'; ' '; strlist];
set(AnalysisData.Handles.editViewInfo, 'string', strlist);


%msgbox(reportTxt);

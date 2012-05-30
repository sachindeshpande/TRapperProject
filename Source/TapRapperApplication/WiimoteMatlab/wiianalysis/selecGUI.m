function varargout = selecGUI(varargin)
gui_Singleton = 1;
gui_State = struct('gui_Name',       mfilename, ...
                   'gui_Singleton',  gui_Singleton, ...
                   'gui_OpeningFcn', @selecGUI_OpeningFcn, ...
                   'gui_OutputFcn',  @selecGUI_OutputFcn, ...
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


% --- Executes just before selecGUI is made visible.
function selecGUI_OpeningFcn(hObject, eventdata, handles, varargin)
global WiiGUIData  TheHandle

handles.output = hObject;

% Update handles structure
guidata(hObject, handles);

%WiiGUIData.rootpath='J:\Projects\WiimoteProjects\WiiMotionPlus\Version29-WithTUIO\';
tpath=pwd;
plst=find(tpath=='\');
WiiGUIData.rootpath=tpath(1:plst(end-1));
WiiGUIData.refpath=[WiiGUIData.rootpath, 'Data\matlab'];
WiiGUIData.datapath=[WiiGUIData.rootpath, 'Data\WiimotePlayData'];
WiiGUIData.currentref=1;
WiiGUIData.LRFlip=0;
WiiGUIData.Warp=0;
addpath([WiiGUIData.rootpath, 'WiimoteMatlab']);
    

reflist=dir([WiiGUIData.refpath, '\*.mat']);
for i1=1:length(reflist)
    tname=reflist(i1).name;
    dotpos=find(tname=='.');
    WiiGUIData.ref{i1}=tname(1:(dotpos(end)-1));
end
set(handles.popupmenuRefList, 'String', WiiGUIData.ref);
WiiGUIData.currentref=1;
loadref(handles);

setDataInfo(handles);


TheHandle=handles;
% UIWAIT makes selecGUI wait for user response (see UIRESUME)
% uiwait(handles.figure1);


function varargout = selecGUI_OutputFcn(hObject, eventdata, handles) 
varargout{1} = handles.output;


function listboxDataFiles_Callback(hObject, eventdata, handles)
global WiiGUIData
WiiGUIData.dataselected=get(hObject, 'Val');
loadplay(handles);

function listboxDataFiles_CreateFcn(hObject, eventdata, handles)
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end


function popupmenuRefList_Callback(hObject, eventdata, handles)
global WiiGUIData
WiiGUIData.currentref=get(hObject, 'Value');
setDataInfo(handles);
loadref(handles);

function popupmenuRefList_CreateFcn(hObject, eventdata, handles)
% global WiiGUIData
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end

function pushbtDataDir_Callback(hObject, eventdata, handles)
global WiiGUIData
datapath=uigetdir(WiiGUIData.datapath, 'Pick a play data directory');
if length(datapath)>1
    WiiGUIData.datapath=datapath;
    setDataInfo(handles);

end


function editDataDir_Callback(hObject, eventdata, handles)
global WiiGUIData
datapath=get(hObject, 'String');
if length(datapath)>1
    WiiGUIData.datapath=datapath;
    setDataInfo(handles);
end

function editDataDir_CreateFcn(hObject, eventdata, handles)
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end

%list datafiles to be displayed
function setDataInfo(handles)
global WiiGUIData
set(handles.editDataDir, 'String', WiiGUIData.datapath);
refname=WiiGUIData.ref{WiiGUIData.currentref};
flist=dir([WiiGUIData.datapath, '\*', refname((end-3):end), '*.csv']);
WiiGUIData.dataselected=0;

if(length(flist)<1)
    WiiGUIData.hasdata=false;
else
    WiiGUIData.hasdata=true;
end

datafilelist{1} = ' ';
for i1=1:length(flist)
    datafilelist{i1}=flist(i1).name;
end
WiiGUIData.datafilelist=datafilelist;

set(handles.listboxDataFiles, 'Val', 1);
set(handles.listboxDataFiles, 'String', datafilelist);

function loadref(handles)
global WiiGUIData
refname=[WiiGUIData.refpath, '\', WiiGUIData.ref{WiiGUIData.currentref}, '.mat'];
load(refname);
WiiGUIData.refend=dataref.time(end);
dataref=cutdata(dataref, dataref.refstart, dataref.playlength);
WiiGUIData.dataref=dataref;

disp(['new reference data loaded: ', WiiGUIData.ref{WiiGUIData.currentref}]);
plotref(handles);

function loadplay(handles)
global WiiGUIData
playname=WiiGUIData.datafilelist{WiiGUIData.dataselected};
playname=[WiiGUIData.datapath, '\', playname];
dataplay=readwiidata(playname);
if isDataOK(dataplay)<1
    return;
end
% if length(dataplay.time)<100
%     msgbox(['Invalid data, length ', num2str(length(dataplay.time))]);
%     return;
% end
dataplay=applyrobustcalibration(dataplay);
WiiGUIData.playend=dataplay.time(end);
offset=WiiGUIData.refend-WiiGUIData.playend;
dataplay=cutdata(dataplay, WiiGUIData.dataref.playstart-offset, WiiGUIData.dataref.playlength);

WiiGUIData.dataplay=dataplay;

disp(['new play data loaded: ', playname]);
plotref(handles);
plottimewarp(handles);
plotplay(handles);


function plotref(handles)
global WiiGUIData

tseq=WiiGUIData.dataref.time;
wiiL=WiiGUIData.dataref.wii1;
wiiR=WiiGUIData.dataref.wii2;
ClearPlot=1;

if not(isempty(handles))
    data=[wiiL.ax'; wiiL.ay'; wiiL.az']+5;
    range=[tseq(1), tseq(end), -10, 10];
    plotonedata(handles.axesAL, tseq, data, range, ClearPlot);
 
    data=[wiiR.ax'; wiiR.ay'; wiiR.az']+5;
    range=[tseq(1), tseq(end), -10, 10];
    plotonedata(handles.axesAR, tseq, data, range, ClearPlot);
    
    data=[wiiL.pitch'; wiiL.roll'; wiiL.yaw']/100.+5;
    range=[tseq(1), tseq(end), -10, 10];
    plotonedata(handles.axesWL, tseq, data, range, ClearPlot);

    data=[wiiR.pitch'; wiiR.roll'; wiiR.yaw']/100.+5;
    range=[tseq(1), tseq(end), -10, 10];
    plotonedata(handles.axesWR, tseq, data, range, ClearPlot);
end

addtaptext(handles);

function plotplay(handles)
global WiiGUIData

tseq=WiiGUIData.dataplay.time;
if WiiGUIData.Warp==1
    tseq=WiiGUIData.warptime;
end

wiiL=WiiGUIData.dataplay.wii1;
wiiR=WiiGUIData.dataplay.wii2;

if WiiGUIData.LRFlip==1
    wiiL=WiiGUIData.dataplay.wii2;
    wiiR=WiiGUIData.dataplay.wii1;
end
ClearPlot=0;

if not(isempty(handles))
    data=[wiiL.ax'; wiiL.ay'; wiiL.az']-5;
    range=[tseq(1), tseq(end), -10, 10];
    plotonedata(handles.axesAL, tseq, data, range, ClearPlot);

    data=[wiiR.ax'; wiiR.ay'; wiiR.az']-5;
    range=[tseq(1), tseq(end), -10, 10];
    plotonedata(handles.axesAR, tseq, data, range, ClearPlot);
    
    data=[wiiL.pitch'; wiiL.roll'; wiiL.yaw']/100.-5;
    range=[tseq(1), tseq(end), -10, 10];
    plotonedata(handles.axesWL, tseq, data, range, ClearPlot);

    data=[wiiR.pitch'; wiiR.roll'; wiiR.yaw']/100.-5;
    range=[tseq(1), tseq(end), -10, 10];
    plotonedata(handles.axesWR, tseq, data, range, ClearPlot);
end


function plotonedata(TheAxes, tseq, data, range, ClearPlot)
axes(TheAxes);
if ClearPlot
    hold off;
end
plot(tseq, data(1,:), 'r'); hold on
plot(tseq, data(2,:), 'g');
plot(tseq, data(3,:), 'b');
grid on
axis(range)


% --- Executes on button press in radiobuttonFlipLR.
function radiobuttonFlipLR_Callback(hObject, eventdata, handles)
global WiiGUIData
WiiGUIData.LRFlip=get(hObject, 'Value');
if WiiGUIData.dataselected>0
    plotref(handles);
    plottimewarp(handles);
    plotplay(handles);
end


% --- Executes on button press in radiobuttonWarp.
function radiobuttonWarp_Callback(hObject, eventdata, handles)
global WiiGUIData
WiiGUIData.Warp=get(hObject, 'Value');
if WiiGUIData.dataselected>0
    plotref(handles);
    plottimewarp(handles);
    plotplay(handles);
end

function varargout = wiidataviewer(varargin)
gui_Singleton = 1;
gui_State = struct('gui_Name',       mfilename, ...
                   'gui_Singleton',  gui_Singleton, ...
                   'gui_OpeningFcn', @wiidataviewer_OpeningFcn, ...
                   'gui_OutputFcn',  @wiidataviewer_OutputFcn, ...
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

function wiidataviewer_OpeningFcn(hObject, eventdata, handles, varargin)
global ViewerData 

ViewerData=[];

handles.output = hObject;

% Update handles structure
guidata(hObject, handles);

tpath=pwd;
plst=find(tpath=='\');
ViewerData.rootpath=tpath(1:plst(end-1));
ViewerData.datapath=['D:\Projects\WiimoteProjects\data\'];
ViewerData.infopath=[ViewerData.rootpath, 'Data\matlab\'];
ViewerData.LRFlip=0;
addpath([ViewerData.rootpath, 'WiimoteMatlab']);
ViewerData.Handles=handles;
ViewerData.Play1=[];
ViewerData.Play2=[];
set(handles.checkboxShowAcc, 'value', 1);
set(handles.checkboxShowGyro, 'value', 1);
set(handles.uipanelAlign,'SelectionChangeFcn',@AlignSelect_ChangeFcn);
ViewerData.Align='AlignEnd';
ViewerData.ApplyCalibration=false;
ViewerData.PlayType='Feet';

function varargout = wiidataviewer_OutputFcn(hObject, eventdata, handles) 
varargout{1} = handles.output;

function pushbuttonSelectData1_Callback(hObject, eventdata, handles)
global ViewerData

filterTxt=get(handles.editFilter, 'String');
[fname, fpath]=uigetfile([ViewerData.datapath, '*', filterTxt, '*.csv'], 'select data 1');
if fname
    ViewerData.Play1.fname=fname;
    ViewerData.datapath=fpath;
    ViewerData.Play1.data_org=readwiidata([fpath, fname]);
    if ViewerData.ApplyCalibration
        ViewerData.Play1.data=applyrobustcalibration(ViewerData.Play1.data_org);
    else
        ViewerData.Play1.data=ViewerData.Play1.data_org;
    end

    infotxt = ['Total ', num2str(ViewerData.Play1.data_org.time(end)), 'ms. '];
    if isfield(ViewerData.Play1.data_org, 'videobegin')
        vbegin=ViewerData.Play1.data_org.videobegin;
        vend=ViewerData.Play1.data_org.videoend;
        infotxt=[infotxt, 'Video start: ', num2str(vbegin), 'ms, end: ',...
            num2str(vend), 'ms, length: ', num2str(vend-vbegin), 'ms'];
    end
    set(handles.editPlay1Info, 'string', infotxt);
    
    showWiiData(ViewerData.Play1, 1, 10);
    set(handles.editData1, 'String', [fpath, fname]);
    
end

function pushbuttonSelectData2_Callback(hObject, eventdata, handles)
global ViewerData

filterTxt=get(handles.editFilter, 'String');
[fname, fpath]=uigetfile([ViewerData.datapath, '*', filterTxt, '*.csv'], 'select data 2');
if fname
    ViewerData.Play2.fname=fname;
    ViewerData.datapath=fpath;
    ViewerData.Play2.data_org=readwiidata([fpath, fname]);
    if ViewerData.ApplyCalibration
        ViewerData.Play2.data=applyrobustcalibration(ViewerData.Play2.data_org);
    else
        ViewerData.Play2.data=ViewerData.Play2.data_org;
    end
    totallen=ViewerData.Play2.data_org.time(end);
    totaldiff=totallen-ViewerData.Play1.data_org.time(end);
    infotxt = ['Total ', num2str(totallen), 'ms(', num2str(totaldiff),').'];
    if isfield(ViewerData.Play2.data_org, 'videobegin')
        vbegin=ViewerData.Play2.data_org.videobegin;
        vend=ViewerData.Play2.data_org.videoend;

        vbegindiff=0; venddiff=0; vlendiff=0;
        if isfield(ViewerData.Play1.data_org, 'videobegin')
            vbegindiff=vbegin-ViewerData.Play1.data_org.videobegin;
            venddiff=vend-ViewerData.Play1.data_org.videoend;;
            vlendiff=venddiff-vbegindiff;
        end
        infotxt=[infotxt, 'Video start: ', num2str(vbegin), 'ms(', num2str(vbegindiff), ') end: ',...
            num2str(vend), 'ms(', num2str(venddiff),'), length: ', num2str(vend-vbegin), 'ms(',num2str(vlendiff),')'];
    end
    set(handles.editPlay2Info, 'string', infotxt);
    AlignData();
    %ViewerData.Play2.data=ViewerData.Play2.data_org;
    showWiiData(ViewerData.Play2, 2, -10);
    set(handles.editData2, 'String', [fpath, fname]);
end

function editData1_CreateFcn(hObject, eventdata, handles)
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end

function editData2_CreateFcn(hObject, eventdata, handles)
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end

function showWiiData(play, player, offset)
global ViewerData
data=play.data;

if isfield(play, 'hwii1AX')
    NewPlot=0;
else
    NewPlot=1;
end

if NewPlot
    axes(ViewerData.Handles.axesDataView);
    
    play.hwii1AX=plot(data.time, data.wii1.ax+5+offset, 'r');
    hold on
    play.hwii1AY=plot(data.time, data.wii1.ay+5+offset, 'g');
    play.hwii1AZ=plot(data.time, data.wii1.az+5+offset, 'b');
    
    play.hwii1Pitch=plot(data.time, data.wii1.pitch/100+2.5+offset, 'r');
    play.hwii1Roll=plot(data.time, data.wii1.roll/100+2.5+offset, 'g');
    play.hwii1Yaw=plot(data.time, data.wii1.yaw/100+2.5+offset, 'b');
    
    play.hwii2AX=plot(data.time, data.wii2.ax-2.5+offset, 'r');
    play.hwii2AY=plot(data.time, data.wii2.ay-2.5+offset, 'g');
    play.hwii2AZ=plot(data.time, data.wii2.az-2.5+offset, 'b');
    
    play.hwii2Pitch=plot(data.time, data.wii2.pitch/100-5+offset, 'r');
    play.hwii2Roll=plot(data.time, data.wii2.roll/100-5+offset, 'g');
    play.hwii2Yaw=plot(data.time, data.wii2.yaw/100-5+offset, 'b');
    
    axis([data.time(1), data.time(end), -10-abs(offset), 10+abs(offset)])
    grid on
    if player==1
        ViewerData.Play1=play;
    else
        ViewerData.Play2=play;
    end
    
else
    set(play.hwii1AX, 'xdata', data.time);
    set(play.hwii1AX, 'ydata', data.wii1.ax+5+offset);
    set(play.hwii1AY, 'xdata', data.time);
    set(play.hwii1AY, 'ydata', data.wii1.ay+5+offset);
    set(play.hwii1AZ, 'xdata', data.time);
    set(play.hwii1AZ, 'ydata', data.wii1.az+5+offset);
    set(play.hwii1Pitch, 'xdata', data.time);
    set(play.hwii1Pitch, 'ydata', data.wii1.pitch/100+2.5+offset);
    set(play.hwii1Roll, 'xdata', data.time);
    set(play.hwii1Roll, 'ydata', data.wii1.roll/100+2.5+offset);
    set(play.hwii1Yaw, 'xdata', data.time);
    set(play.hwii1Yaw, 'ydata', data.wii1.yaw/100+2.5+offset);
    
    set(play.hwii2AX, 'xdata', data.time);
    set(play.hwii2AX, 'ydata', data.wii2.ax-2.5+offset);
    set(play.hwii2AY, 'xdata', data.time);
    set(play.hwii2AY, 'ydata', data.wii2.ay-2.5+offset);
    set(play.hwii2AZ, 'xdata', data.time);
    set(play.hwii2AZ, 'ydata', data.wii2.az-2.5+offset);
    set(play.hwii2Pitch, 'xdata', data.time);
    set(play.hwii2Pitch, 'ydata', data.wii2.pitch/100-5+offset);
    set(play.hwii2Roll, 'xdata', data.time);
    set(play.hwii2Roll, 'ydata', data.wii2.roll/100-5+offset);
    set(play.hwii2Yaw, 'xdata', data.time);
    set(play.hwii2Yaw, 'ydata', data.wii2.yaw/100-5+offset);
end
 
ShowAcc=get(ViewerData.Handles.checkboxShowAcc, 'value');
if not(ShowAcc)
    set(play.hwii1AX, 'xdata', []);
    set(play.hwii1AX, 'ydata', []);
    set(play.hwii1AY, 'xdata', []);
    set(play.hwii1AY, 'ydata', []);
    set(play.hwii1AZ, 'xdata', []);
    set(play.hwii1AZ, 'ydata', []);
    set(play.hwii2AX, 'xdata', []);
    set(play.hwii2AX, 'ydata', []);
    set(play.hwii2AY, 'xdata', []);
    set(play.hwii2AY, 'ydata', []);
    set(play.hwii2AZ, 'xdata', []);
    set(play.hwii2AZ, 'ydata', []);
end
    
ShowGyro=get(ViewerData.Handles.checkboxShowGyro, 'value');
if not(ShowGyro)
    set(play.hwii1Pitch, 'xdata', []);
    set(play.hwii1Pitch, 'ydata', []);
    set(play.hwii1Roll, 'xdata', []);
    set(play.hwii1Roll, 'ydata', []);
    set(play.hwii1Yaw, 'xdata', []);
    set(play.hwii1Yaw, 'ydata', []);
    set(play.hwii2Pitch, 'xdata', []);
    set(play.hwii2Pitch, 'ydata', []);
    set(play.hwii2Roll, 'xdata', []);
    set(play.hwii2Roll, 'ydata', []);
    set(play.hwii2Yaw, 'xdata', []);
    set(play.hwii2Yaw, 'ydata', []);
end

function pushbuttonZoomIn_Callback(hObject, eventdata, handles)
axes(handles.axesDataView);
ax=axis(handles.axesDataView);

x1=ax(1); x2=ax(2);
xc=round((x1+x2)/2);
xlen=round((x2-x1)/4);
x1=xc-xlen;
x2=xc+xlen;

axis([x1, x2, ax(3), ax(4)]);

function pushbuttonZoomOut_Callback(hObject, eventdata, handles)
axes(handles.axesDataView);
ax=axis(handles.axesDataView);

x1=ax(1); x2=ax(2);
xc=round((x1+x2)/2);
xlen=x2-x1;
x1=xc-xlen;
x2=xc+xlen;

axis([x1, x2, ax(3), ax(4)]);

function pushbuttonCenter_Callback(hObject, eventdata, handles)
axes(handles.axesDataView);
ax=axis(handles.axesDataView);
xc=round((ax(2)+ax(1))/2);
[x, y]=ginput(1);

x0=ax(1)-xc+round(x);
xend=ax(2)-xc+round(x);
axis([x0, xend, ax(3), ax(4)]);

function pushbuttonRange_Callback(hObject, eventdata, handles)
axes(handles.axesDataView);
ax=axis(handles.axesDataView);
[x, y]=ginput(2);
x0=round(min(x));
xend=round(max(x));
axis([x0, xend, ax(3), ax(4)]);

function pushbuttonMeasure_Callback(hObject, eventdata, handles)
axes(handles.axesDataView);
[x, y]=ginput(2);
x0=round(min(x));
xend=round(max(x));
 msgbox([num2str(x0), '-', num2str(xend), ', duration: ', num2str(xend-x0), 'ms']);

function editFilter_Callback(hObject, eventdata, handles)

function editFilter_CreateFcn(hObject, eventdata, handles)

if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end

function checkboxShowAcc_Callback(hObject, eventdata, handles)
UpdateView();

function checkboxShowGyro_Callback(hObject, eventdata, handles)
UpdateView();

function UpdateView()
global ViewerData
if isfield(ViewerData.Play1, 'data')
    showWiiData(ViewerData.Play1, 1, 10);
end
if isfield(ViewerData.Play2, 'data')
    showWiiData(ViewerData.Play2, 2, -10);
end

function checkboxCalibrate_Callback(hObject, eventdata, handles)
global ViewerData
ViewerData.ApplyCalibration=get(hObject, 'value');


if ViewerData.ApplyCalibration
    ViewerData.Play1.data=applyrobustcalibration(ViewerData.Play1.data_org);
    ViewerData.Play2.data=applyrobustcalibration(ViewerData.Play2.data_org);
else
    ViewerData.Play1.data=ViewerData.Play1.data_org;
    ViewerData.Play2.data=ViewerData.Play2.data_org;
end
AlignData();
UpdateView();

function checkboxShowBarInfo_Callback(hObject, eventdata, handles)
DoShow=get(hObject, 'Value');
if DoShow
    ShowBarInfo;
else
    HideBarInfo;
end

function AlignSelect_ChangeFcn(hObject, eventdata)
global ViewerData

handles = guidata(hObject); 
switch get(eventdata.NewValue,'Tag')   % Get Tag of selected object
    case 'radiobuttonAlignEnd'
      ViewerData.Align='AlignEnd';      
    case 'radiobuttonAlignBegin'
      ViewerData.Align='AlignBegin';
    otherwise
end
if isfield(ViewerData.Play2, 'data_org')
    AlignData();
end
UpdateView();

function AlignData()
global ViewerData
play2shift=str2num(get(ViewerData.Handles.editPlay2Shift, 'String'));

if isfield(ViewerData.Play2, 'data')
    switch ViewerData.Align
        case 'AlignEnd'
            Play1end=ViewerData.Play1.data.time(end);
            Play2end=ViewerData.Play2.data_org.time(end);
            offset=Play1end-Play2end+play2shift;
        case 'AlignBegin'
            offset=play2shift;
    end
    ViewerData.Play2.data.time=ViewerData.Play2.data_org.time+offset;
end

function editPlay2Shift_Callback(hObject, eventdata, handles)
AlignData();
UpdateView();

function editPlay2Shift_CreateFcn(hObject, eventdata, handles)
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end

function editPlay1Info_Callback(hObject, eventdata, handles)

function editPlay1Info_CreateFcn(hObject, eventdata, handles)
% hObject    handle to editPlay1Info (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    empty - handles not created until after all CreateFcns called

% Hint: edit controls usually have a white background on Windows.
%       See ISPC and COMPUTER.
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end

function editPlay2Info_Callback(hObject, eventdata, handles)

function editPlay2Info_CreateFcn(hObject, eventdata, handles)
% hObject    handle to editPlay2Info (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    empty - handles not created until after all CreateFcns called

% Hint: edit controls usually have a white background on Windows.
%       See ISPC and COMPUTER.
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end


function editPlayInfoFname_Callback(hObject, eventdata, handles)

function editPlayInfoFname_CreateFcn(hObject, eventdata, handles)
% hObject    handle to editPlayInfoFname (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    empty - handles not created until after all CreateFcns called

% Hint: edit controls usually have a white background on Windows.
%       See ISPC and COMPUTER.
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end

function editNumBar_Callback(hObject, eventdata, handles)
global ViewerData
if(isfield(ViewerData, 'hBarLines'))
    HideBarInfo;
    if length(ViewerData.hBarLines)<ViewerData.BPB
        ViewerData=rmfield(ViewerData, 'hBarLines');
    end
end


if get(handles.checkboxShowBarInfo, 'Value')
    ShowBarInfo;
end


function editNumBar_CreateFcn(hObject, eventdata, handles)
% hObject    handle to editNumBar (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    empty - handles not created until after all CreateFcns called

% Hint: edit controls usually have a white background on Windows.
%       See ISPC and COMPUTER.
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end



function editPlayStart_Callback(hObject, eventdata, handles)
if get(handles.checkboxShowBarInfo, 'Value')
    ShowBarInfo;
end

function editPlayStart_CreateFcn(hObject, eventdata, handles)
% hObject    handle to editPlayStart (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    empty - handles not created until after all CreateFcns called

% Hint: edit controls usually have a white background on Windows.
%       See ISPC and COMPUTER.
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end


function editPlayLength_Callback(hObject, eventdata, handles)
if get(handles.checkboxShowBarInfo, 'Value')
    ShowBarInfo;
end

function editPlayLength_CreateFcn(hObject, eventdata, handles)
% hObject    handle to editPlayLength (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    empty - handles not created until after all CreateFcns called

% Hint: edit controls usually have a white background on Windows.
%       See ISPC and COMPUTER.
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end


function editBPM_Callback(hObject, eventdata, handles)
if get(handles.checkboxShowBarInfo, 'Value')
    ShowBarInfo;
end

function editBPM_CreateFcn(hObject, eventdata, handles)
% hObject    handle to editBPM (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    empty - handles not created until after all CreateFcns called

% Hint: edit controls usually have a white background on Windows.
%       See ISPC and COMPUTER.
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end

function ShowBarInfo
global ViewerData
handles=ViewerData.Handles;


ViewerData.NumBar=str2num(get(handles.editNumBar, 'String'));
ViewerData.BPB=4;
ViewerData.PlayStart=str2num(get(handles.editPlayStart, 'String'));
ViewerData.PlayLength=str2num(get(handles.editPlayLength, 'String'));
ViewerData.Beat=ViewerData.PlayLength/ViewerData.NumBar/ViewerData.BPB;
ViewerData.BPM=60*1000/ViewerData.Beat;

idx=get(handles.popupmenuPlayOffset, 'value');
strlist=get(handles.popupmenuPlayOffset, 'string');

ViewerData.PlayOffset=str2num(strlist{idx});

set(handles.editBPM, 'String', num2str(ViewerData.BPM));

axes(handles.axesDataView);
if not(isfield(ViewerData, 'hBarLines'))
    for i1=1:100        %maximum 25 bars
        ViewerData.hBarLines(i1)=plot(0, 0);
        ViewerData.hStepMarks(i1)=text(-10, 0, ' ');
    end
    ViewerData.hRefStartMark=plot(0, 0, 'd','LineWidth',2);
    ViewerData.hRefEndMark=plot(0, 0, 'd','LineWidth',2);    
end

for i1=1:(ViewerData.NumBar*ViewerData.BPB+1)
    x=ViewerData.PlayStart+ViewerData.Beat*(i1-1);
    set(ViewerData.hBarLines(i1), 'xdata', [x, x]);
    set(ViewerData.hBarLines(i1), 'ydata', [-20, 20]);
    if ((i1-1)-floor((i1-1)/ViewerData.BPB)*ViewerData.BPB)==0
        set(ViewerData.hBarLines(i1), 'color', 'm');
    else
        set(ViewerData.hBarLines(i1), 'color', 'c');
    end
end
set(ViewerData.hRefStartMark, 'xdata',...
    ViewerData.PlayStart-ViewerData.PlayOffset*ViewerData.Beat);
set(ViewerData.hRefEndMark, 'xdata',...
    ViewerData.PlayStart-ViewerData.PlayOffset*ViewerData.Beat+ViewerData.PlayLength);


if isfield(ViewerData, 'StepMark')
    StepMark=ViewerData.StepMark;
    x=ViewerData.PlayStart+(StepMark.bpos-1)*ViewerData.Beat;
    for i1=1:length(x)
        switch StepMark.feet{i1}
            case 'L'
                y=2;
            case 'R'
                y=15;
            case 'Both'
                y=8.5;
        end
        set(ViewerData.hStepMarks(i1), 'String', StepMark.mark(i1));
        set(ViewerData.hStepMarks(i1), 'Position', [x(i1), y, 0]);
        set(ViewerData.hStepMarks(i1), 'Rotation', 90);
        
    end    
end

function HideBarInfo
global ViewerData
handles=ViewerData.Handles;
axes(handles.axesDataView);
if isfield(ViewerData, 'hBarLines')
    for i1=1:length(ViewerData.hBarLines)
        set(ViewerData.hBarLines(i1), 'xdata', []);
        set(ViewerData.hBarLines(i1), 'ydata', []);
        set(ViewerData.hStepMarks(i1), 'string', ' ');
    end
end


% --- Executes on button press in pushbuttonSavePlayInfo.
function pushbuttonSavePlayInfo_Callback(hObject, eventdata, handles)
global ViewerData
playtypestring=get(handles.editFilter, 'string');
[fname, fpath]=uiputfile([ViewerData.infopath, 'info*',playtypestring,'*.mat'], 'Save Play Information');
if fname
    playinfo.NumBar=ViewerData.NumBar;
    playinfo.PlayStart=ViewerData.PlayStart;
    playinfo.PlayLength=ViewerData.PlayLength;
    playinfo.BPM=ViewerData.BPM;
    playinfo.BPB=ViewerData.BPB;
    playinfo.Beat=ViewerData.Beat;
    playinfo.PlayOffset=ViewerData.PlayOffset;
    
    save([fpath, fname], 'playinfo');
end

function pushbuttonLoadPlayInfo_Callback(hObject, eventdata, handles)
global ViewerData

playtypestring=get(handles.editFilter, 'string');
[fname, fpath]=uigetfile([ViewerData.infopath, 'info*',playtypestring,'*.mat'], 'load Play Information');
if fname
    load([fpath, fname]);
    
    %ViewerData.playinfo=playinfo;
    ViewerData.NumBar=playinfo.NumBar;
    ViewerData.BPB=4;
    ViewerData.PlayStart=playinfo.PlayStart;
    ViewerData.PlayLength=playinfo.PlayLength;
    if isfield(playinfo, 'Beat')
        ViewerData.Beat=playinfo.Beat;
    end
    if not(isfield(playinfo, 'PlayOffset'))
        playinfo.PlayOffset=0;
    end
    ViewerData.PlayOffset=playinfo.PlayOffset;
    
    ViewerData.BPM=playinfo.BPM;
    set(handles.editNumBar, 'String', num2str(ViewerData.NumBar));
    set(handles.editPlayStart, 'String', num2str(ViewerData.PlayStart));
    set(handles.editPlayLength, 'String', num2str(ViewerData.PlayLength));
    set(handles.editBPM, 'String', num2str(ViewerData.BPM));
    strlist=get(handles.popupmenuPlayOffset, 'string');
    for i1=1:length(strlist)
        if strcmp(num2str(playinfo.PlayOffset), strlist{i1})
            idx=i1;
        end
    end

    set(handles.popupmenuPlayOffset, 'value', idx);

    [fname, fpath]=uigetfile([fpath, 'info*',playtypestring,'*.csv'], 'Get step marks');
    if fname
        fid=fopen([fpath, fname]);
        C=textscan(fid, '%f%s%s', 'Delimiter', ',' );
        fclose(fid);
        for i1=1:length(C)
            StepMark.bpos=C{1};
            StepMark.feet=C{2};
            StepMark.mark=C{3};
        end
        ViewerData.StepMark=StepMark;
    end
    HideBarInfo;
    ShowBarInfo;
    set(ViewerData.Handles.checkboxShowBarInfo, 'value', 1);
end

% --- Executes on button press in pushbuttonSaveReference.
function pushbuttonSaveReference_Callback(hObject, eventdata, handles)
global ViewerData
playtypestring=get(handles.editFilter, 'string');
[fname, fpath]=uiputfile([ViewerData.infopath, 'ref*', playtypestring,'*.mat'], 'Save Play1 as Reference');
if fname
    dataref=applyrobustcalibration(ViewerData.Play1.data_org);
    %dataref.data_org=ViewerData.Play1.data_org;
    dataref.BPM=ViewerData.BPM;
    dataref.BPB=ViewerData.BPB;
    dataref.NumBar=ViewerData.NumBar;
    dataref.playstart=ViewerData.PlayStart; %+ 1 beat
    dataref.playlength=ViewerData.PlayLength;% +2 beat
    dataref.refstart=dataref.playstart;
    dataref.originfname=ViewerData.Play1.fname;
    dataref.fname=fname;
    save([fpath, fname], 'dataref');
end

function pushbuttonLoadReference_Callback(hObject, eventdata, handles)
global ViewerData

playtypestring=get(handles.editFilter, 'string');
[fname, fpath]=uigetfile([ViewerData.infopath, 'ref*',playtypestring,'*.mat'], 'Load Reference to play1');
if fname
    load([fpath, fname]);
    if isfield(dataref, 'data_org')
        ViewerData.Play1.data_org=dataref.data_org;
    else
        ViewerData.Play1.data_org=dataref;
    end
    
    ViewerData.BPM=dataref.BPM;
    ViewerData.BPB=dataref.BPB;
    
    ViewerData.NumBar=dataref.NumBar;
    ViewerData.PlayStart=dataref.playstart; %- 1 beat
    ViewerData.PlayLength=dataref.playlength;% -2 beat
    ViewerData.Play1.fname=dataref.originfname;
    fname=dataref.fname;
    
    if ViewerData.ApplyCalibration
        ViewerData.Play1.data=applyrobustcalibration(ViewerData.Play1.data_org);
    else
        ViewerData.Play1.data=ViewerData.Play1.data_org;
    end

    infotxt = ['Total ', num2str(ViewerData.Play1.data_org.time(end)), 'ms. '];
    if isfield(ViewerData.Play1.data_org, 'videobegin')
        vbegin=ViewerData.Play1.data_org.videobegin;
        vend=ViewerData.Play1.data_org.videoend;
        infotxt=[infotxt, 'Video start: ', num2str(vbegin), 'ms, end: ',...
            num2str(vend), 'ms, length: ', num2str(vend-vbegin), 'ms'];
    end
    set(handles.editPlay1Info, 'string', infotxt);
    
    showWiiData(ViewerData.Play1, 1, 10);
    set(handles.editData1, 'String', [fname, '<---', dataref.originfname]);
end


% --- Executes on selection change in popupmenuPlayType.
function popupmenuPlayType_Callback(hObject, eventdata, handles)
global ViewerData
PlayTypeList=get(hObject, 'string');
Playidx=get(hObject, 'value');
switch PlayTypeList{Playidx}
    case 'Feet'
        set(handles.editFilter, 'string', 'feet');
    case 'ToeStand'
        set(handles.editFilter, 'string', 'toe');
    case 'PopOver'
        set(handles.editFilter, 'string', 'pop');
end
ViewerData.PlayType=PlayTypeList(Playidx);

function popupmenuPlayType_CreateFcn(hObject, eventdata, handles)
% hObject    handle to popupmenuPlayType (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    empty - handles not created until after all CreateFcns called

% Hint: popupmenu controls usually have a white background on Windows.
%       See ISPC and COMPUTER.
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end


% --- Executes on selection change in popupmenuplayoffset.
function popupmenuPlayOffset_Callback(hObject, eventdata, handles)
% hObject    handle to popupmenuplayoffset (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

% Hints: contents = get(hObject,'String') returns popupmenuplayoffset contents as cell array
%        contents{get(hObject,'Value')} returns selected item from popupmenuplayoffset


% --- Executes during object creation, after setting all properties.
function popupmenuPlayOffset_CreateFcn(hObject, eventdata, handles)
% hObject    handle to popupmenuplayoffset (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    empty - handles not created until after all CreateFcns called

% Hint: popupmenu controls usually have a white background on Windows.
%       See ISPC and COMPUTER.
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end


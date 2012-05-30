
function data=readwiidata(fname)

%fname='D:\Projects\WiimoteProjects\WiiMotionPlus\Version26-WithTUIO\Data\WiimoteReferenceData\Reference6_Try_2.csv';
%[fname, fpath]=uigetfile('*.csv');

%fid=fopen('DropSet01WiiData.csv');

%fid=fopen([fpath, fname]);
fid=fopen(fname);

readheader;

C=textscan(fid, '%d%d%f%f%f%f%f%s%s%s%f%f%f%f%f%f%f%f%s%s%s%f%f%f', 'Delimiter', ',' );
fclose(fid);

numdata=length(C{end});
data.orglength=numdata;

data.time=C{2}(1:numdata);

%find video begin and end
data.seq=C{1}(1:numdata);

idx=find(data.seq==-1);
if idx
    data.videobegin=data.time(idx(end)+1)-5;
end
idx=find(data.seq==-9);
if idx
    data.videoend=data.time(idx-1)+5;
end



data.wii1.accpitch=C{3}(1:numdata);
data.wii1.accroll=C{4}(1:numdata);
data.wii1.ax=C{5}(1:numdata);
data.wii1.ay=C{6}(1:numdata);
data.wii1.az=C{7}(1:numdata);

data.wii1.yaw=C{11}(1:numdata);
data.wii1.pitch=C{12}(1:numdata);
data.wii1.roll=C{13}(1:numdata);

data.wii2.accpitch=C{14}(1:numdata);
data.wii2.accroll=C{15}(1:numdata);
data.wii2.ax=C{16}(1:numdata);
data.wii2.ay=C{17}(1:numdata);
data.wii2.az=C{18}(1:numdata);

data.wii2.yaw=C{22}(1:numdata);
data.wii2.pitch=C{23}(1:numdata);
data.wii2.roll=C{24}(1:numdata);

%set wiimote translation matrix
data.wii1.TransMat=[1 0 0;0 1 0; 0 0 1];
data.wii2.TransMat=[1 0 0;0 1 0; 0 0 1];
data.CalMatSet=false;
data.CalApplied=false;

if(isfield(data.wii1, 'cala')) % calibration exist
    if sum(data.wii1.cala.*data.wii1.cala+data.wii2.cala.*data.wii2.cala)>0.1
        data.wii1.TransMat=g2transmat(data.wii1.cala);
        data.wii2.TransMat=g2transmat(data.wii2.cala);% [x, y, z] in columes
    end
    data.CalMatSet=true;
end
    
if isfield(data, 'LRSwitched')
    if data.LRSwitched==1
        wiit=data.wii1;
        data.wii1=data.wii2;
        data.wii2=wiit;
        data.LRSwitched=0;
        disp('LRSwitched');
    end
end

%sorting the data
tt=data.time;
t1=tt(2:end)-tt(1:end-1);
lst=find(t1<0);
while not(isempty(lst))
    idx=lst(1);
    temp=data.time(idx);
    data.time(idx)=data.time(idx+1);
    data.time(idx+1)=temp;
    
   
    temp=data.wii1.ax(idx); 
    data.wii1.ax(idx)=data.wii1.ax(idx+1);
    data.wii1.ax(idx+1)=temp;
    
    temp=data.wii1.ay(idx); 
    data.wii1.ay(idx)=data.wii1.ay(idx+1);
    data.wii1.ay(idx+1)=temp;
    
    temp=data.wii1.az(idx); 
    data.wii1.az(idx)=data.wii1.az(idx+1);
    data.wii1.az(idx+1)=temp;
    
    temp=data.wii1.yaw(idx); 
    data.wii1.yaw(idx)=data.wii1.yaw(idx+1);
    data.wii1.yaw(idx+1)=temp;
    
    temp=data.wii1.pitch(idx); 
    data.wii1.pitch(idx)=data.wii1.pitch(idx+1);
    data.wii1.pitch(idx+1)=temp;
    
    temp=data.wii1.roll(idx); 
    data.wii1.roll(idx)=data.wii1.roll(idx+1);
    data.wii1.roll(idx+1)=temp;
    
    temp=data.wii1.accpitch(idx); 
    data.wii1.accpitch(idx)=data.wii1.accpitch(idx+1);
    data.wii1.accpitch(idx+1)=temp;
    
    temp=data.wii1.accroll(idx); 
    data.wii1.accroll(idx)=data.wii1.accroll(idx+1);
    data.wii1.accroll(idx+1)=temp;
    
    
    
    temp=data.wii2.ax(idx); 
    data.wii2.ax(idx)=data.wii2.ax(idx+1);
    data.wii2.ax(idx+1)=temp;
    
    temp=data.wii2.ay(idx); 
    data.wii2.ay(idx)=data.wii2.ay(idx+1);
    data.wii2.ay(idx+1)=temp;
    
    temp=data.wii2.az(idx); 
    data.wii2.az(idx)=data.wii2.az(idx+1);
    data.wii2.az(idx+1)=temp;
    
    
    temp=data.wii2.yaw(idx); 
    data.wii2.yaw(idx)=data.wii2.yaw(idx+1);
    data.wii2.yaw(idx+1)=temp;
    
    temp=data.wii2.pitch(idx); 
    data.wii2.pitch(idx)=data.wii2.pitch(idx+1);
    data.wii2.pitch(idx+1)=temp;
    
    temp=data.wii2.roll(idx); 
    data.wii2.roll(idx)=data.wii2.roll(idx+1);
    data.wii2.roll(idx+1)=temp;
    
    temp=data.wii2.accpitch(idx); 
    data.wii2.accpitch(idx)=data.wii2.accpitch(idx+1);
    data.wii2.accpitch(idx+1)=temp;
    
    temp=data.wii2.accroll(idx); 
    data.wii2.accroll(idx)=data.wii2.accroll(idx+1);
    data.wii2.accroll(idx+1)=temp;
    
    tt=data.time;
    t1=tt(2:end)-tt(1:end-1);
    lst=find(t1<0);
end

%drop data have 0 duration
lst=find(t1==0);
if not(isempty(lst))
    range=setdiff(1:length(tt), (lst+1));
    data.time=data.time(range);
    data.wii1.ax=data.wii1.ax(range);
    data.wii1.ay=data.wii1.ay(range);
    data.wii1.az=data.wii1.az(range);
    data.wii1.yaw=data.wii1.yaw(range);
    data.wii1.pitch=data.wii1.pitch(range);
    data.wii1.roll=data.wii1.roll(range);
    data.wii1.accpitch=data.wii1.accpitch(range);
    data.wii1.accroll=data.wii1.accroll(range);
    
    data.wii2.ax=data.wii2.ax(range);
    data.wii2.ay=data.wii2.ay(range);
    data.wii2.az=data.wii2.az(range);
    data.wii2.yaw=data.wii2.yaw(range);
    data.wii2.pitch=data.wii2.pitch(range);
    data.wii2.roll=data.wii2.roll(range);
    data.wii2.accpitch=data.wii2.accpitch(range);
    data.wii2.accroll=data.wii2.accroll(range);
    
end

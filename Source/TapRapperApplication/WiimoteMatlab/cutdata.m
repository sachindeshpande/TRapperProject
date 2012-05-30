function dataout=cutdata(datain, tstart, tlength)
dataout=datain;
%DeltaT=5;
%tstart=0; tend=dataref.time(end)
tend=tstart+tlength;
%range=round(tstart/DeltaT+1):round(tend/DeltaT+1);
tmin=max(find(datain.time<tstart));
tmax=min(find(datain.time>tend));
if isempty(tmin)
    tmin=1;
end
if isempty(tmax)
    tmax=length(datain.time);
end

range=tmin:tmax;

dataout.wii1.ax=dataout.wii1.ax(range);
dataout.wii1.ay=dataout.wii1.ay(range);
dataout.wii1.az=dataout.wii1.az(range);

dataout.wii1.yaw=dataout.wii1.yaw(range);
dataout.wii1.pitch=dataout.wii1.pitch(range);
dataout.wii1.roll=dataout.wii1.roll(range);

dataout.wii2.ax=dataout.wii2.ax(range);
dataout.wii2.ay=dataout.wii2.ay(range);
dataout.wii2.az=dataout.wii2.az(range);

dataout.wii2.yaw=dataout.wii2.yaw(range);
dataout.wii2.pitch=dataout.wii2.pitch(range);
dataout.wii2.roll=dataout.wii2.roll(range);

%rlength=length(range);
%dataout.time=1:rlength;
dataout.time=dataout.time(range);
dataout.time=dataout.time-dataout.time(1);
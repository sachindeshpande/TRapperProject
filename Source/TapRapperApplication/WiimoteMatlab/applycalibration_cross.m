function dataout=applycalibration_cross(data)
calw=1;
if calw
    data.wii1.yaw=data.wii1.yaw-data.wii1.calw(1);
    data.wii1.pitch=data.wii1.pitch-data.wii1.calw(2);
    data.wii1.roll=data.wii1.roll-data.wii1.calw(3);

    data.wii2.yaw=data.wii2.yaw-data.wii2.calw(1);
    data.wii2.pitch=data.wii2.pitch-data.wii2.calw(2);
    data.wii2.roll=data.wii2.roll-data.wii2.calw(3);
end

TransMat=data.wii1.TransMat;
data.wii1.TransMat=data.wii2.TransMat;
data.wii2.TransMat=data.wii1.TransMat;

dataout=data;
T1=[data.wii1.ax, data.wii1.ay, data.wii1.az];
NewT1=T1*data.wii1.TransMat;
dataout.wii1.ax=NewT1(:,1);
dataout.wii1.ay=NewT1(:,2);
dataout.wii1.az=NewT1(:,3);

T1=[data.wii1.pitch, data.wii1.roll, data.wii1.yaw];
NewT1=T1*data.wii1.TransMat;
dataout.wii1.pitch=NewT1(:,1);
dataout.wii1.roll=NewT1(:,2);
dataout.wii1.yaw=NewT1(:,3);


T1=[data.wii2.ax, data.wii2.ay, data.wii2.az];
NewT1=T1*data.wii2.TransMat;
dataout.wii2.ax=NewT1(:,1);
dataout.wii2.ay=NewT1(:,2);
dataout.wii2.az=NewT1(:,3);

T1=[data.wii2.pitch, data.wii2.roll, data.wii2.yaw];
NewT1=T1*data.wii2.TransMat;
dataout.wii2.pitch=NewT1(:,1);
dataout.wii2.roll=NewT1(:,2);
dataout.wii2.yaw=NewT1(:,3);

dataout.CalApplied=true;
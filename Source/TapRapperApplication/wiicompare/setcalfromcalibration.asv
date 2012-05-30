function dataout=setcalfromcalibration(datain, calfile)

%calfile='D:\Projects\WiimoteProjects\WiiMotionPlus\Version26-WithTUIO\Data\WiimoteReferenceData\data2010May10\StandStill.csv';
data=readwiidata(calfile);

gdata=[data.wii1.ax'; data.wii1.ay'; data.wii1.az'; ...
    data.wii1.pitch'; data.wii1.roll'; data.wii1.yaw'; ...
    data.wii2.ax'; data.wii2.ay'; data.wii2.az'; ...
    data.wii2.pitch'; data.wii2.roll'; data.wii2.yaw'];

gmean=mean(gdata');

wii1a=gmean(1:3);
wii2a=gmean(7:9);

dataout=datain;
dataout.wii1TransMat=g2transmat(wii1a);
dataout.wii2TransMat=g2transmat(wii2a);% [x, y, z] in columes
dataout.CalMatSet=true;

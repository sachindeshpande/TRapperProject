%fpath=pwd;
[fname, fpath]=uigetfile('*.csv', 'Open Wii Data', [fpath, '\a.csv']);
data=readwiidata([fpath, fname]);
showwiidata(data);
subplot(2,2,1); title('No calibration');

pause(2);
data=applycalibration(data);
showwiidata(data);
subplot(2,2,1); title('calibration from csv');

pause(2);
data=applyrobustcalibration(data);
showwiidata(data);
subplot(2,2,1); title('robust calibration');



%fpath=pwd;
if(not(exist('fpath')))
    fpath=pwd;
    i1=1;
end
[fname, fpath]=uigetfile('*.csv', 'Open Wii Data', [fpath, '\a.csv']);
data1=readwiidata([fpath, fname]);
figure(1); clf; 
offset=10;
showwiidata(data1, offset);

[fname, fpath]=uigetfile('*.csv', 'Open Wii Data', [fpath, '\a.csv']);
data2=readwiidata([fpath, fname]);

offset=-10;
showwiidata(data2, offset);




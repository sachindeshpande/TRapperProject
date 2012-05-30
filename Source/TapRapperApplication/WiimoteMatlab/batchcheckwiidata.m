fpath=pwd;
addpath(fpath);

fpath=uigetdir(fpath);

cd(fpath)
flist=dir('*toe*')
clear tend tlength
for i1=1:length(flist)
    data=readwiidata([fpath, '\', flist(i1).name]);
    showwiidata(data);
    subplot(2,2,1); title('No calibration');
    
%     pause(2);
%     data=applycalibration(data);
%     showwiidata(data);
%     subplot(2,2,1); title('calibration from csv');
%     
%     pause(2);
%     data=applyrobustcalibration(data);
%     showwiidata(data);
%     subplot(2,2,1); title('robust calibration');
%     
    
    disp(data.time(end))
    tend(i1)=data.time(end);
    tlength(i1)=data.orglength;
end


clf
plot(tend)
disp([max(tend), min(tend), max(tend)-min(tend), mean(tend)])
disp([max(tlength), min(tlength), max(tlength)-min(tlength), mean(tlength)])

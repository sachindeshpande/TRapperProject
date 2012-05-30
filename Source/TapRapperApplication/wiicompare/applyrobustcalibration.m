function data=applyrobustcalibration(data)
%data=dataplay
% showwiidata(data);
% datawithcal=applycalibration(data);
% showwiidata(datawithcal)


tlen=length(data.time);
tstart=1;
tend=tstart+100;
ical=1;
while tend<tlen
    range=tstart:tend;
    variation(ical)=...
        std(data.wii1.ax(range))+std(data.wii1.ay(range))+std(data.wii1.az(range))+...
        (std(data.wii1.yaw(range))+std(data.wii1.pitch(range))+std(data.wii1.roll(range)))/500+...
        std(data.wii2.ax(range))+std(data.wii2.ay(range))+std(data.wii2.az(range))+...
        (std(data.wii2.yaw(range))+std(data.wii2.pitch(range))+std(data.wii2.roll(range)))/500;
    tstart=tstart+50; tend=tstart+100;
    ical=ical+1;
end
%clf; plot(variation);

[val, idx]=min(variation);
tstart=1+(idx-1)*50; tend=tstart+100;
range=tstart:tend;
disp(['select range: ', num2str(data.time(tstart)), '  ', num2str(data.time(tend)), ' variation: ', num2str(val)]);

newcala1=[mean(data.wii1.ax(range)), mean(data.wii1.ay(range)), mean(data.wii1.az(range))];
newcala2=[mean(data.wii2.ax(range)), mean(data.wii2.ay(range)), mean(data.wii2.az(range))];
newcalw1=[mean(data.wii1.yaw(range)), mean(data.wii1.pitch(range)), mean(data.wii1.roll(range))];
newcalw2=[mean(data.wii2.yaw(range)), mean(data.wii2.pitch(range)), mean(data.wii2.roll(range))];
newcala1=newcala1/sqrt(sum(newcala1.*newcala1));
newcala2=newcala2/sqrt(sum(newcala2.*newcala2));

if val<0.1
    oldcala1=data.wii1.cala;
    oldcala2=data.wii2.cala;
    oldcala1=oldcala1/sqrt(sum(oldcala1.*oldcala1));
    oldcala2=oldcala2/sqrt(sum(oldcala2.*oldcala2));
    simi=sum([newcala1 newcala2].*[oldcala1 oldcala2]);
    disp(['simi to exist: ', num2str(simi), ' threshold: ', num2str(1.97)]);
    if simi<1.97
        disp('calibration updated');
        data.wii1.cala=newcala1;
        data.wii2.cala=newcala2;
        data.wii1.calw=newcalw1;
        data.wii2.calw=newcalw2;
        data.wii1.TransMat=g2transmat(data.wii1.cala);
        data.wii2.TransMat=g2transmat(data.wii2.cala);% [x, y, z] in columes
    end
end
data=applycalibration(data);
% showwiidata(data)

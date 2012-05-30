

be4play=cutdata(dataplay,  0, dataref.playstart);
be4playcal=applycalibration(be4play);
showwiidata(be4playcal)


tlen=length(be4play.time)
tstart=1;
tend=tstart+50;
ical=1;
while tend<tlen
    range=tstart:tend;
    variation(ical)=...
        std(be4play.wii1.ax(range))+std(be4play.wii1.ay(range))+std(be4play.wii1.az(range))+...
        (std(be4play.wii1.yaw(range))+std(be4play.wii1.pitch(range))+std(be4play.wii1.roll(range)))/500+...
        std(be4play.wii2.ax(range))+std(be4play.wii2.ay(range))+std(be4play.wii2.az(range))+...
        (std(be4play.wii2.yaw(range))+std(be4play.wii2.pitch(range))+std(be4play.wii2.roll(range)))/500;
    tstart=tstart+25; tend=tstart+50;
    ical=ical+1;
end
clf; plot(variation)
[val, idx]=min(variation)
tstart=1+(idx-1)*25; tend=tstart+50;

newcala1=[mean(be4play.wii1.ax(range)), mean(be4play.wii1.ay(range)), mean(be4play.wii1.az(range))]
newcala2=[mean(be4play.wii2.ax(range)), mean(be4play.wii2.ay(range)), mean(be4play.wii2.az(range))]
newcala1=newcala1/sqrt(sum(newcala1.*newcala1));
newcala2=newcala2/sqrt(sum(newcala2.*newcala2));

if val<0.3
    oldcala1=be4play.wii1.cala;
    oldcala2=be4play.wii1.cala;
    oldcala1=oldcala1/sqrt(sum(oldcala1.*oldcala1));
    oldcala2=oldcala2/sqrt(sum(oldcala2.*oldcala2));
    simi=sum([newcala1 newcala2].*[oldcala1 oldcala2])
    if simi<1.96
        disp('calibration needs to be updated');
        dataplay.wii1.cala=newcala1;
        dataplay.wii2.cala=newcala2;
    end
end
dataplay.wii1.TransMat=g2transmat(dataplay.wii1.cala);
dataplay.wii2.TransMat=g2transmat(dataplay.wii2.cala);% [x, y, z] in columes

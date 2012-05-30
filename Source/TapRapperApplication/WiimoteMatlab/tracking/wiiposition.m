rootpath='D:\Projects\WiimoteProjects\TRapperProject\trunk\Source\TapRapperApplication\'
mpath=[rootpath, 'WiimoteMatlab\'];
datapath=[rootpath, 'Data\matlab'];
datapath='D:\Projects\WiimoteProjects\data\June11_trackingtest';
homepath=[mpath, 'tracking\']
addpath(mpath);


[fname, fpath]=uigetfile('*.csv', ['Open Data'], [datapath, '\a.csv']);
fnameplay=[fpath, fname];

%cd(datapath)
%load toe_stand
%load pop_over
%cd(homepath)

dataorg=readwiidata(fnameplay);
datacal=applyrobustcalibration(dataorg);
data=setaccpry(dataorg);

shoe=[0 -4, 0; 0.5 -3 0; 0.5 1 0; -0.5 1 0; -0.5, -3 0; 0, -4, 0]';
shoe=[shoe+repmat([0,0,0.25]', 1,6), shoe+repmat([0,0,-0.25]', 1,6)];


wii1avep=-atan((datacal.wii1.cala(2))./sqrt((datacal.wii1.cala(1)).^2+(datacal.wii1.cala(3)).^2));
wii1aver=atan2((datacal.wii1.cala(1)),(datacal.wii1.cala(3)));
wii1avey=0;
wii2avep=-atan((datacal.wii2.cala(2))./sqrt((datacal.wii2.cala(1)).^2+(datacal.wii2.cala(3)).^2));
wii2aver=atan2((datacal.wii2.cala(1)),(datacal.wii2.cala(3)));
wii2avey=0;
wii1rot=ypr2rot(wii1avey, wii1avep, wii1aver);
wii2rot=ypr2rot(wii2avey, wii2avep, wii2aver);
shoe1=wii1rot^-1*shoe;
shoe2=wii2rot^-1*shoe;




clf
subplot(4,1,1)
plot(data.wii1.accpitch); hold on
plot(data.wii1.accpitchcal*180/pi, 'r');
subplot(4,1,2)
plot(data.wii1.accroll); hold on
plot(data.wii1.accrollcal*180/pi, 'r');
subplot(4,1,3)
plot(data.wii2.accpitch); hold on
plot(data.wii2.accpitchcal*180/pi, 'r');
subplot(4,1,4)
plot(data.wii2.accroll); hold on
plot(data.wii2.accrollcal*180/pi, 'r');


data.wii1.yaw=data.wii1.yaw-datacal.wii1.calw(1);
data.wii1.pitch=data.wii1.pitch-datacal.wii1.calw(2);
data.wii1.roll=data.wii1.roll-datacal.wii1.calw(3);

data.wii2.yaw=data.wii2.yaw-datacal.wii2.calw(1);
data.wii2.pitch=data.wii2.pitch-datacal.wii2.calw(2);
data.wii2.roll=data.wii2.roll-datacal.wii2.calw(3);






T=double(data.time)/1000.;
deltaT=T(2:end)-T(1:end-1);
deltaT=repmat(deltaT, 1, 3);

wii=data.wii1;
A=[wii.ax'; wii.ay'; wii.az'];
Weight1=0.05./(1+(sum(A.^2)-1).^2);
W=[wii.pitch'; wii.roll'; wii.yaw'];
aveW=(W(:,1:end-1)+W(:,2:end))/2.*deltaT';
aveA=(A(:,1:end-1)+A(:,2:end))/2;
cosW=cos(aveW*pi/180.);
sinW=sin(aveW*pi/180.);

sinW1=sinW; cosW1=cosW; AveA1=aveA;

wii=data.wii2;
A=[wii.ax'; wii.ay'; wii.az'];
Weight2=0.05./(1+(sum(A.^2)-1).^2);
W=[wii.pitch'; wii.roll'; wii.yaw'];
aveW=(W(:,1:end-1)+W(:,2:end))/2.*deltaT';
aveA=(A(:,1:end-1)+A(:,2:end))/2;
cosW=cos(aveW*pi/180.);
sinW=sin(aveW*pi/180.); AveA2=aveA;

sinW2=sinW; cosW2=cosW;



D1=ypr2rot(data.wii1.accyawcal(1), data.wii1.accpitchcal(1), data.wii1.accrollcal(1))
D2=ypr2rot(data.wii2.accyawcal(1), data.wii2.accpitchcal(1), data.wii2.accrollcal(1))

%PX=[1, 0 ,0]'; PY=[0, 1, 0]'; PZ=[0, 0, 1]';
%D1=[PX,PY,PZ]; D2=D1;
%D1=[0,0,1;0,-1,0;1,0,0]'; %for test1, test2
%D1=[0,-1,0;0,0,-1;1,0,0]';
%D2=[1,0,0; 0,1,0; 0,0,1]';

% [y, p, r]=acc2ypr(data.wii1.ax(1), data.wii1.ay(1), data.wii1.az(1))
% [y, p, r]=rot2ypr(D1)
%[y, p, r]=acc2ypr(data.wii2.ax(1), data.wii2.ay(1), data.wii2.az(1))

P1=[0, 0, 0]'; P2=P1;
V1=[0, 0, 0]'; V2=V1;




fignum=2; tstart=0; tlength=data.time(end)-data.time(1); numbar=0; beatperbar=100; PlotGyro=1; Subplot=0;
figure(fignum); clf;
plotwiidata(fignum, data, tstart, tlength, numbar, beatperbar, PlotGyro, Subplot)
subplot(4,1,1); hold off
plot(data.time, data.wii1.accpitch); hold on
plot(data.time, data.wii1.accpitchcal*180/pi, 'r'); hold on
plot(data.time, Weight1*1000, 'g');
subplot(4,1,2); hold off
plot(data.time, data.wii2.accpitch); hold on
plot(data.time, data.wii2.accpitchcal*180/pi, 'r'); hold on
plot(data.time, Weight2*1000, 'g');




figure(1); clf; pcol='rgb';

for i1=1:length(aveW)
    
    dT=deltaT(i1,1);
    
    MPitch1=[1, 0, 0; 0, cosW1(1,i1), -sinW1(1,i1); 0, sinW1(1,i1), cosW1(1,i1)];
    MRoll1=[cosW1(2,i1), 0, sinW1(2,i1); 0, 1, 0; -sinW1(2,i1), 0, cosW1(2, i1)];
    MYaw1=[cosW1(3,i1), -sinW1(3,i1), 0; sinW1(3, i1), cosW1(3,i1), 0; 0, 0, 1];
    D1=D1*MYaw1*MPitch1*MRoll1;
    [y, p, r]=rot2ypr(D1);
    if(abs(r-data.wii1.accrollcal(i1))>pi)
        rcal=data.wii1.accrollcal(i1);
        pcal=data.wii1.accpitchcal(i1);
        disp([i1/10000, 1, r, rcal, 180/pi*(r-rcal), p, pcal,180/pi*(p-pcal), Weight1(i1)]);
    end
    
    p=(1-Weight1(i1))*p+Weight1(i1)*data.wii1.accpitchcal(i1);
    r=(1-Weight1(i1))*r+Weight1(i1)*data.wii1.accrollcal(i1);
    y=0.98*y;
    D1=ypr2rot(y, p, r);
    %disp([i1, 1, y, p, r]);
    
    V1=10*(D1*AveA1(:,i1)-[0,0,1.]')*dT+V1;
    P1=0.9*(V1*dT*0.4+P1);
    
    MPitch2=[1, 0, 0; 0, cosW2(1,i1), -sinW2(1,i1); 0, sinW2(1,i1), cosW2(1,i1)];
    MRoll2=[cosW2(2,i1), 0, sinW2(2,i1); 0, 1, 0; -sinW2(2,i1), 0, cosW2(2, i1)];
    MYaw2=[cosW2(3,i1), -sinW2(3,i1), 0; sinW2(3, i1), cosW2(3,i1), 0; 0, 0, 1];
    D2=D2*MYaw2*MPitch2*MRoll2;

    [y, p, r]=rot2ypr(D2);
    p=(1-Weight2(i1))*p+Weight2(i1)*data.wii2.accpitchcal(i1);
    if(abs(r-data.wii2.accrollcal(i1))>pi)
        rcal=data.wii2.accrollcal(i1);
        pcal=data.wii2.accpitchcal(i1);
        disp([i1/10000, 1, r, rcal, 180/pi*(r-rcal), p, pcal,180/pi*(p-pcal), Weight2(i1)]);
    end
    
    r=(1-Weight2(i1))*r+Weight2(i1)*data.wii2.accrollcal(i1);
    y=0.98*y;
    D2=ypr2rot(y, p, r);
    %disp([i1, 2, y, p, r]);
    
    V2=10*(D2*AveA2(:,i1)-[0,0,1.]')*dT+V2;
    P2=0.9*(V2*dT*0.4+P2);
    
    if (i1- 25* round(i1/25))==0
        figure(1);
        subplot(1,2,1);
        hold off
         stem3(P1(1)+4, P1(2)+4, P1(3)+4, '^g', 'filled');
       hold on
        for i2=1:3
            plot3([0, D1(1,i2)]+4, [0, D1(2,i2)]+4, [0, D1(3, i2)]+4, pcol(i2));
        end
        plot3([0, -4*D1(1,2)]+4, [0, -4*D1(2,2)]+4, [0, -4*D1(3, 2)]+4, 'k');
        tshoe=D1*shoe1+4;
        plot3(tshoe(1,:), tshoe(2,:), tshoe(3,:), 'k')
        
        axis([-2 2 -2 2 -2 2]*2+4);
        view([180+5, 30]);
        title('X red, Y greed, Z, blue');
        
        
        
        subplot(1,2,2);
        hold off
        stem3(P2(1)+4, P2(2)+4, P2(3)+4, '^g', 'filled'); hold on
        for i2=1:3
            plot3([0, D2(1,i2)]+4, [0, D2(2,i2)]+4, [0, D2(3, i2)]+4, pcol(i2));
        end
        plot3([0, -4*D2(1,2)]+4, -[0, 4*D2(2,2)]+4, -[0, 4*D2(3, 2)]+4, 'k');
        tshoe=D2*shoe2+4;
        plot3(tshoe(1,:), tshoe(2,:), tshoe(3,:), 'k')
       
       
        axis([-2 2 -2 2 -2 2]*2+4);
        view([180-5, 30]);
        title(num2str(i1));
        
        figure(2);
        for isub =1:4
            subplot(4,1,isub);
            plot(data.time(i1), 0, '+');
        end
        
        pause(0.1);
        
        
    end
end




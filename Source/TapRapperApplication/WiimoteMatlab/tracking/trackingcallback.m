function trackingcallback()
global wiimoteData wiimoteDataSave wii 

if (length(wiimoteData)>5)
    tend=wiimoteData(end,2);
    tbegin=wiimoteData(1,2);
    %disp([tbegin, tend]);
    if(not(exist('wiimoteDataSave')))
        wiimoteDataSave=wiimoteData;
    else
        wiimoteDataSave=[wiimoteDataSave; wiimoteData];
    end
    
    if tbegin<1000
        clf;
        clear wiimoteDataSave
        disp('init');
    elseif tbegin<4000
        disp('calibration');
        wiimean=mean(wiimoteDataSave);
        wii.amean=wiimean(5:7);
        wii.wmean=wiimean(8:10);
        
        acc=wiimoteData(end, 5:7);
        wii.deltaT=0.008015;
        
        [y, p, r]=acc2ypr(acc);
        wii.yaw=y; wii.pitch=p; wii.roll=r;
        disp(wii);
        updatewii(tend);
    else
        %disp('updating');
        updatewii(tend);
        %plotshoewithwiipr();
        plotshoewithacc();
        %plotshoewithgyro();
        plotshoegyroNacc();
    end
 

    wiimoteData=0;
end
end

function updatewii(tend)
global wiimoteData

subplot(4,1,1);
plot(wiimoteData(:,2), wiimoteData(:,5), 'r'); hold on
plot(wiimoteData(:,2), wiimoteData(:,6), 'r'); hold on
plot(wiimoteData(:,2), wiimoteData(:,7), 'r'); hold on
axis([tend-10000, tend, -5 5]);

subplot(4,1,2);
plot(wiimoteData(:,2), wiimoteData(:,13), 'r'); hold on
plot(wiimoteData(:,2), wiimoteData(:,14), 'r'); hold on
plot(wiimoteData(:,2), wiimoteData(:,15), 'r'); hold on
axis([tend-10000, tend, -5 5]);

drawnow; pause(0.005);
end

function plotshoewithwiipr()
global wiimoteData
shoe=[0 -4, 0; 0.5 -3 0; 0.5 1 0; -0.5 1 0; -0.5, -3 0; 0, -4, 0]';
shoe=[shoe+repmat([0,0,0.25]', 1,6), shoe+repmat([0,0,-0.25]', 1,6)];

pitch=wiimoteData(end, 3)*pi/180;
roll=wiimoteData(end, 4)*pi/180;
Rot=ypr2rot(0, pitch, roll);


subplot(2, 2, 3)
tshoe=Rot*shoe+4;
stem3(4,4,4); hold on
plot3(tshoe(1,:), tshoe(2,:), tshoe(3,:), 'k');
hold off
axis([-2 2 -2 2 -2 2]*2+4);
view([180-5, 30]);
end


function plotshoewithacc()
global wiimoteData
shoe=[0 -4, 0; 0.5 -3 0; 0.5 1 0; -0.5 1 0; -0.5, -3 0; 0, -4, 0]';
shoe=[shoe+repmat([0,0,0.25]', 1,6), shoe+repmat([0,0,-0.25]', 1,6)];


acc=wiimoteData(end, 5:7);
Rot=acc2rot(acc);

subplot(2, 2, 3)
tshoe=Rot*shoe+4;
stem3(4,4,4); hold on
plot3(tshoe(1,:), tshoe(2,:), tshoe(3,:), 'k');
hold off
axis([-2 2 -2 2 -2 2]*2+4);
view([180-5, 30]);
end

function plotshoewithgyro()
global wiimoteData wii
shoe=[0 -4, 0; 0.5 -3 0; 0.5 1 0; -0.5 1 0; -0.5, -3 0; 0, -4, 0]';
shoe=[shoe+repmat([0,0,0.25]', 1,6), shoe+repmat([0,0,-0.25]', 1,6)];

rot=ypr2rot(wii.yaw, wii.pitch, wii.roll);
for i1=1:size(wiimoteData, 1)
    dypr=(wiimoteData(i1, 8:10)-wii.wmean)*pi/180*wii.deltaT;
    drot=ypr2rot(dypr(1), dypr(2), dypr(3));
    rot=rot*drot;
end

subplot(2, 2, 4)
tshoe=rot*shoe+4;
stem3(4,4,4); hold on
plot3(tshoe(1,:), tshoe(2,:), tshoe(3,:), 'k');
hold off
axis([-2 2 -2 2 -2 2]*2+4);
view([180-5, 30]);
end

function plotshoegyroNacc()
global wiimoteData wii
shoe=[0 -4, 0; 0.5 -3 0; 0.5 1 0; -0.5 1 0; -0.5, -3 0; 0, -4, 0]';
shoe=[shoe+repmat([0,0,0.25]', 1,6), shoe+repmat([0,0,-0.25]', 1,6)];

for i1=1:size(wiimoteData, 1)
    dypr=(wiimoteData(i1, 8:10)-wii.wmean)*pi/180*wii.deltaT;
    drot=ypr2rot(dypr(1), dypr(2), dypr(3));
    rot=ypr2rot(wii.yaw, wii.pitch, wii.roll);
    rot=rot*drot;
    old_ypr=[wii.yaw, wii.pitch, wii.roll];
    [y, p, r]=rot2ypr(rot); ypr=[y, p, r];
    acc=wiimoteData(i1, 5:7);
    [accy, accp, accr]=acc2ypr(acc); acc_ypr=[accy, accp, accr];
    weight=0.05/(1+(sum(acc.^2)-1)^2);
    D_ypr=ypr-old_ypr;
    D_acc_ypr=acc_ypr-old_ypr;
    new_ypr=old_ypr + (1-weight)*D_ypr + weight * D_acc_ypr;
    wii.yaw=ypr(1); wii.pitch=new_ypr(2); wii.roll=new_ypr(3);
    
    
end

rot=ypr2rot(wii.yaw, wii.pitch, wii.roll);
subplot(2, 2, 4)
tshoe=rot*shoe+4;
stem3(4,4,4); hold on
plot3(tshoe(1,:), tshoe(2,:), tshoe(3,:), 'k');
hold off
axis([-2 2 -2 2 -2 2]*2+4);
view([180-5, 30]);


end


function RotMat=ypr2rot(yaw, pitch, roll)

MPitch=[1, 0, 0; 0, cos(pitch), sin(pitch); 0, -sin(pitch), cos(pitch)];
MRoll=[cos(roll), 0, -sin(roll); 0, 1, 0; sin(roll), 0, cos(roll)];
MYaw=[cos(yaw), sin(yaw), 0; -sin(yaw), cos(yaw), 0; 0, 0, 1];
RotMat=MYaw*MPitch*MRoll;

end

function [y, p, r]=acc2ypr(acc)
ax=acc(1); ay=acc(2); az=acc(3);
p=-atan(ay./sqrt(ax.^2+az.^2));
r=atan2(ax,az);
y=0;

end
function [yaw, pitch, roll]=rot2ypr(rot)
pitch=-atan(rot(3,2)/sqrt(rot(3,1)^2+rot(3,3)^2)); %-Wx @ z/
yaw=atan2(rot(1,2),rot(2,2)); %WX @y / WX @x assume pitch < pi/2 is ture
roll=atan2(rot(3,1),rot(3,3));
end

function RotMat=acc2rot(acc)

ax=acc(1); ay=acc(2); az=acc(3);
pitch=-atan(ay./sqrt(ax.^2+az.^2));
roll=atan2(ax,az);
yaw=0;

RotMat=ypr2rot(yaw, pitch, roll);
end
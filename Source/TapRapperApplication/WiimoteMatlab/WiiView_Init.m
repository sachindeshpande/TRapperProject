function ViewHandle=WiiView_Init(Axes)


cla(Axes);
axes(Axes);

ViewHandle.href=WiiView_initAPlayPlot(Axes);
ViewHandle.hplay=WiiView_initAPlayPlot(Axes);
ViewHandle.hbar=WiiView_initBarLines(Axes);
ViewHandle.hwarp=WiiView_initWarpLines(Axes);
ViewHandle.hstep=WiiView_initStep(Axes);

axis([-2, 1, -20, 20]);

function hplay=WiiView_initAPlayPlot(Axes)
axes(Axes);
hplay.hwii1AX=plot(-1,0, 'r');
hold on
hplay.hwii1AY=plot(-1,1, 'g');
hplay.hwii1AZ=plot(-1,2, 'b');

hplay.hwii1Pitch=plot(-1,0, 'r');
hplay.hwii1Roll=plot(-1,0, 'g');
hplay.hwii1Yaw=plot(-1,0, 'b');

hplay.hwii2AX=plot(-1,0, 'r');
hplay.hwii2AY=plot(-1,0, 'g');
hplay.hwii2AZ=plot(-1,0, 'b');

hplay.hwii2Pitch=plot(-1,0, 'r');
hplay.hwii2Roll=plot(-1,0, 'g');
hplay.hwii2Yaw=plot(-1,0, 'b');

WiiView_clearPlot(hplay, 'Both');

function hbar=WiiView_initBarLines(Axes)
hbar=zeros(1,100);
axes(Axes);

for i1=1:100
    if (i1-1-floor((i1-1)/4)*4)==0
        pcolor='r';
    else
        pcolor='c';
    end
    hbar(i1)=plot([-10, -10], [-20, 20], pcolor);    
end

function hwarp=WiiView_initWarpLines(Axes)
hwarp=zeros(1,256);
axes(Axes);

for i1=1:256
    hwarp(i1)=plot([-10, -10], [10, -10], 'm');    
end

function hstep=WiiView_initStep(Axes)
axes(Axes);
for i1=1:20
    hstep(i1).hpline=line([-10, -10], [10, 10], 'color', 'r', 'LineWidth', 2);
    hstep(i1).hqline=line([-10, -10], [-10, -10], 'color', 'r', 'LineWidth', 2);
    hstep(i1).hwarp=line([-10, -10], [10, -10], 'color', 'r', 'LineWidth', 2);
end
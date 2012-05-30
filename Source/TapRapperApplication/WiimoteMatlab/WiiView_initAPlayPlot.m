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
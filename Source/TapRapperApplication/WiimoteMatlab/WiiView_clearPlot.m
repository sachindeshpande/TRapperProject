function WiiView_clearPlot(hplay, type)
ClearAcc=false;
ClearGyro=false;
switch type
    case 'Acc'
        ClearAcc=true;
    case 'Gyro'
        ClearGyro=true;
    case 'Both'
        ClearAcc=true;
        ClearGyro=true;
end

if ClearAcc
    set(hplay.hwii1AX, 'xdata', []);
    set(hplay.hwii1AX, 'ydata', []);
    set(hplay.hwii1AY, 'xdata', []);
    set(hplay.hwii1AY, 'ydata', []);
    set(hplay.hwii1AZ, 'xdata', []);
    set(hplay.hwii1AZ, 'ydata', []);
    set(hplay.hwii2AX, 'xdata', []);
    set(hplay.hwii2AX, 'ydata', []);
    set(hplay.hwii2AY, 'xdata', []);
    set(hplay.hwii2AY, 'ydata', []);
    set(hplay.hwii2AZ, 'xdata', []);
    set(hplay.hwii2AZ, 'ydata', []);
end
if ClearGyro
    set(hplay.hwii1Pitch, 'xdata', []);
    set(hplay.hwii1Pitch, 'ydata', []);
    set(hplay.hwii1Roll, 'xdata', []);
    set(hplay.hwii1Roll, 'ydata', []);
    set(hplay.hwii1Yaw, 'xdata', []);
    set(hplay.hwii1Yaw, 'ydata', []);
    set(hplay.hwii2Pitch, 'xdata', []);
    set(hplay.hwii2Pitch, 'ydata', []);
    set(hplay.hwii2Roll, 'xdata', []);
    set(hplay.hwii2Roll, 'ydata', []);
    set(hplay.hwii2Yaw, 'xdata', []);
    set(hplay.hwii2Yaw, 'ydata', []);
end

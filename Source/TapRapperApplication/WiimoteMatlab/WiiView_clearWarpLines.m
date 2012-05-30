function WiiView_clearWarpLines(hwarp)

for i1=1:length(hwarp)
    set(hwarp(i1), 'xdata', [-10, -10]);
end
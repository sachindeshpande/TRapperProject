function WiiView_clearBarMark(hbar)

for i1=1:length(hbar)
    set(hbar, 'xdata', [-10, -10]);
end
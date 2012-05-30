function WiiView_clearStep(hstep)

for i1=1:length(hstep)
    set(hstep(i1).hpline, 'xdata', [-10, -10]);
    set(hstep(i1).hqline, 'xdata', [-10, -10]);
    set(hstep(i1).hwarp, 'xdata', [-10, -10]);
end
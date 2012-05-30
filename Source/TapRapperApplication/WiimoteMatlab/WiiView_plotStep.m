function WiiView_plotStep(hstep, step)

for i1=1:length(step)
    plotAStep(hstep(i1), step(i1));
end

function plotAStep(hstep, step)

set(hstep.hpline, 'xdata', [step.left, step.right]);
set(hstep.hqline, 'xdata', [step.qleft, step.qright]);
set(hstep.hwarp, 'xdata', [step.center, step.center+step.offset]);

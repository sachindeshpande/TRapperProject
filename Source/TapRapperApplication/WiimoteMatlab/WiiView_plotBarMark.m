function WiiView_plotBarMark(hbar, NumBar, BPB, Beat)
numlines=NumBar*BPB+1;
for i1=1:numlines
    x=Beat*(i1-1);
    set(hbar(i1), 'xdata', [x x]);
end

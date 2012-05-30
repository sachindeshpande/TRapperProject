function WiiView_plotWarpLines(hwarp, PlayLength, Beat, Space, p, q)

numStep=max(max(p), max(q))-1;
stepSpacing=PlayLength/numStep;

numBeat=round(PlayLength/Beat);
numWarpLine=numBeat/Space;
warpSpacing=round(numStep/numWarpLine);

xp=1:warpSpacing:(numStep+1);

for i1=(1:numStep+1)
    idxlst=find(i1==p);
    pnew=i1;
    if idxlst
        qnew(i1)=mean(q(idxlst));
    else
        qnew(i1)=i1;
    end
end

xq=qnew(xp);

xp=(xp-1)*stepSpacing;
xq=(xq-1)*stepSpacing;

for i1=1:length(xp)
    set(hwarp(i1), 'xdata', [xp(i1), xq(i1)]);
end
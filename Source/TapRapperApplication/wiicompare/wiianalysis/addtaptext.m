function addtaptext(handles)
global WiiGUIData

switch WiiGUIData.currentref
    case 1
        return;
    case 2
        %popovers
        Lseq={'u',' ', 'over', ' ', ' ','u', 'back',' ', ' ', 'k', ' ', ' ', 's', ' ', ' ', ' '}
        Rseq={'pop',' ',' ', ' ', 'pop', ' ', ' ', ' ', '->','k','d', ' ', ' ', 'p', 't', ' '}
        skip=580; endcut=-230;
    case 3
        %toestand
        skip=230; endcut=0;
        Lseq={'s', 'tip', ' ', 'd', ' ', ' ', 'F', 'R', 's', 'tip', ' ', 'd', 'x', ' ', 'R', ' '};
        Rseq={'s', 'tip', ' ', 'd', 'F', 'R', ' ', ' ', 's', 'tip', ' ', 'd', 'x', ' ', 'R', ' '};
end

tbegin=WiiGUIData.dataref.time(1)+skip;
tend=WiiGUIData.dataref.time(end)-endcut;
NumB=16;
Repeat=2;

tmark=tbegin:(tend-tbegin)/(NumB*Repeat):tend;

for ax=[handles.axesAL, handles.axesAR, handles.axesWL, handles.axesWR]
    axes(ax);
    for i1=1:NumB
        text(double(tmark(i1)),-0.5, Lseq{i1}, 'FontSize', 12, 'FontWeight', 'bold');
        text(double(tmark(i1+NumB)),-0.5, Lseq{i1}, 'FontSize', 12, 'FontWeight', 'bold');
    end
end

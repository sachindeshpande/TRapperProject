function addmatchline(handles, p, q, c, tsequence);
global WiiGUIData 


playtype=WiiGUIData.ref{WiiGUIData.currentref};

switch playtype
    case 'feet'
        xmark=[0.0049, 0.2117, 0.4053, 0.5727, 0.7433, 0.8910, 1.1109, 1.2586, 1.3833,...
            1.4784, 1.6130, 1.8165, 1.9707, 2.1676, 2.4269, 2.5253, 2.7616]*10000.;
        WiiGUIData.dataref.segmentbase=ones(1, 16); % to be replaced
    case 'toe_stand' 
        xmark=[0.1079, 0.8918, 1.3576, 1.9256, 2.6073, 3.3003, 4.0615, 4.8681, 5.6179, 6.1291,...
            6.7540, 7.2538, 8.0264, 8.4126, 8.8103, 9.5146]*1000.;
    case 'pop_over'
        xmark=[0.3576, 1.4473, 2.6846, 3.8765, 5.0116, 6.2603, 7.5203, 8.6100, 9.5068]*1000.;
end

%[xmark, y]=ginput()
    
ptime=tsequence(p);
qtime=tsequence(q);

idx=0;
for i1=1:length(xmark)
    [val, idx(i1)]=min(abs(ptime-xmark(i1)));
end

for i1=1:length(p)
    cost(i1)=c(p(i1), q(i1));
end

for i1=1:(length(xmark)-1)
    subcost(i1)=cost(idx(i1+1))-cost(idx(i1));
end
subcost=subcost./WiiGUIData.dataref.segmentbase;



x=[ptime(idx)', qtime(idx)'];
if WiiGUIData.Warp==1
    x=[ptime(idx)', ptime(idx)'];
end
y=repmat([5, -5], length(idx),1);

for ax=[handles.axesAL, handles.axesAR, handles.axesWL, handles.axesWR]
    axes(ax);
    plot(x', y', 'k')
%     for i1=1:length(xmark)
%         line([ptime(idx(i1)), qtime(idx(i1))], [5, -5], 'Color', 'Black');
%     end
end

threshold=[-1000, 2, 3, 4];
ax=handles.axesAL;
for i1=1:(length(xmark)-1)
    tst=num2str(subcost(i1));
    if length(tst)>4
        tst=tst(1:4);
    end
    idx=find(subcost(i1)>threshold);
    pcolor={'Green', 'Blue', 'Yellow', 'Red'};
    text((x(i1+1,1)+x(i1,1))/2, 36, tst, 'Color', pcolor{idx(end)});
end


% detailed matching analysis

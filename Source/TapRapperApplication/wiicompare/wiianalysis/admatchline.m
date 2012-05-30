axes(handles.axesAL);
ptime=tsequence(p(1:200:end));
qtime=tsequence(q(1:200:end));

for i1=1:length(ptime)
    line([ptime(i1), qtime(i1)], [5, -5]);
end
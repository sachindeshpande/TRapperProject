function plotwiidata(fignum, data, tstart, tlength, numbar, beatperbar, PlotGyro, Subplot)

tseq=data.time;
scala=5; scalw=500;
nump=numbar*beatperbar;

idxstart=max(find(tseq<tstart));
if isempty(idxstart)
    idxstart=1;
else
    idxstart=idxstart+1;
end
if idxstart>length(tseq)
    idxstart=length(tseq);
end

idxend=min(find(tseq>(tstart+tlength)));
if isempty(idxend)
    idxend=length(tseq);
else
    idxend=idxend-1;
end
if idxend < 1
    idxend = 1;
end
range=idxstart:idxend;

for i1=1:(nump+1)
    px(i1)=tstart+(i1-1)*tlength/nump;
end

figure(fignum) 
subplot(4,1,1+Subplot);
plot(tseq(range), data.wii1.ax(range), 'r');
hold on
plot(tseq(range), data.wii1.ay(range), 'g');
plot(tseq(range), data.wii1.az(range), 'b');
axis([tseq(range(1)), tseq(range(end)), -scala, scala]);


subplot(4,1,2+Subplot);
plot(tseq(range), data.wii2.ax(range), 'r');
hold on
plot(tseq(range), data.wii2.ay(range), 'g');
plot(tseq(range), data.wii2.az(range), 'b');
axis([tseq(range(1)), tseq(range(end)), -scala, scala]);

if PlotGyro
    subplot(4,1,3);
    plot(tseq(range), data.wii1.yaw(range), 'r');
    hold on
    plot(tseq(range), data.wii1.pitch(range), 'g');
    plot(tseq(range), data.wii1.roll(range), 'b');
    axis([tseq(range(1)), tseq(range(end)), -scalw, scalw]);
    
    subplot(4,1,4);
    plot(tseq(range), data.wii2.yaw(range), 'r');
    hold on
    plot(tseq(range), data.wii2.pitch(range), 'g');
    plot(tseq(range), data.wii2.roll(range), 'b');
    axis([tseq(range(1)), tseq(range(end)), -scalw, scalw]);
end

NumPlot=2;
if PlotGyro
    NumPlot=4;
end

for iplot=1:NumPlot
    subplot(4,1,iplot+Subplot);
    for iline=1:(nump+1)
        h=line([px(iline), px(iline)], [-scalw, scalw]);
        if (iline+3)/4==floor((iline+3)/4)
            set(h, 'Color', 'red');
            %disp(iline)
        else
            set(h, 'Color', 'black');
        end
    end
end

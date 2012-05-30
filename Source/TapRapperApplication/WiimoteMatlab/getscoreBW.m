function [score, PScore, Pmask, p, q, c, sm]=getscoreBW(dataref, dataplay, FAST, PlayWindow)
DoPlot=false;
%data already normalized and interpolated

sm=1-simmx(dataref, dataplay);
sm_org=sm;
% sm=sm.^1.;
% sm=(sm-0.5)*2;
th=0.175; %<th accept, >th reject
for i1=1:size(sm, 1)
    for i2=1:size(sm,2)
        if sm(i1,i2)<th
            sm(i1,i2)=0;
        else sm(i1,i2)=1;
        end
    end
end

tic
if FAST
    %[p, q, c]=dpfast(1-sm);
    Cstep = [1 1 1.0;0 1 1.0;1 0 1.0];
    %Cstep=[1 1 1;1 0 1;0 1 1;1 2 2;2 1 2];
    T=1; G=0.5;
    
    [p, q, c]=dpfastwindow(sm, Cstep, T, G, PlayWindow);
else
    %[p, q, c]=dp2(1-sm);
    [p, q, c]=dp2window(sm, PlayWindow);
end
toc
tic
[i10, i20]=size(c);
for i1=1:i10
    for i2=1:i20
        if c(i1, i2)>1000
            c(i1, i2)=0;
        end
    end
end

toc

for i1=1:length(p)
    %Pixelcost(i1)=c(p(i1+1), q(i1+1))-c(p(i1), q(i1));
    Pixelcost(i1)=sm(p(i1),q(i1));
    Pixelcost_org(i1)=sm_org(p(i1),q(i1));
end

PScore=zeros(1, length(dataref));
PScore_Overlap=PScore;
PScore_Raw=PScore;
for i1=1:length(p)
    PScore_Raw(p(i1))=PScore_Raw(p(i1))+Pixelcost(i1);
    PScore_Overlap(p(i1))=PScore_Overlap(p(i1))+1;
end



%mask out region with low activity, using 0.
refpower=sum(dataref.*dataref);
mask_idx=find(refpower<3);
Pmask=ones(1, length(dataref));
Pmask(mask_idx)=0;

PScore=round(PScore_Raw./PScore_Overlap);
for i1=1:length(PScore)
    if PScore_Overlap(i1)==0
        disp(['Skipped ref point at ', num2str(i1)]);
        PScore(i1)=0;
        Pmask(i1)=0;
    end
end

score_withoutMask=100-sum(PScore)/length(PScore)*100;
PScore=PScore.*Pmask; %0 accept, 1 reject
accepted=Pmask&(not(PScore));
%score=100-sum(PScore)/(sum(Pmask))*100;

score=sum(accepted)/sum(Pmask)*100;

CheckAcception=1;
if(CheckAcception)
    
    playpower0=sum(dataplay.*dataplay);
    playpower=0*playpower0;
    for i1=1:length(p)
        playpower(p(i1))=playpower0(q(i1));
    end
    
    ActivityMask=(sqrt(refpower)-1)./(sqrt(playpower)-1);
    
    for i1=1:length(ActivityMask);
        if ActivityMask(i1)>3;
            ActivityMask(i1)=0; %0 reject
        else
            ActivityMask(i1)=1;
        end
    end
    %    ActivityMask=round(ActivityMask/3);
    
    if DoPlot
        figure(2); clf
        plot(sqrt(refpower)-1, 'k'); hold on
        %plot(sqrt(playpower0)-1, 'b');
        plot(sqrt(playpower)-1, 'r');
        plot(accepted*5, 'm');
        plot(Pmask*5+6, 'b');
        plot(ActivityMask*3, 'g');
        legend('ref', 'warped play', 'accepted', 'ref mask for high act', 'play mask for high act');
        
    end
    scoreWithActivityMask=sum(accepted.*ActivityMask)/sum(Pmask)*100;
    %disp([score_withoutMask, score, scoreWithActivityMask]);
    score=scoreWithActivityMask;
end
%powermask;


i1=1;




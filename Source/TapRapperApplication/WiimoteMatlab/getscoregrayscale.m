function [score, p, q, c, sm]=getscoregrayscale(dataref, dataplay, FAST, SubSample, NumBar, PlayWindow)
%data already normalized and interpolated

sm=1-simmx(dataref, dataplay);


  
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
score=c(size(c,1), size(c,2))/NumBar/SubSample/mean(mean(dataref));

    
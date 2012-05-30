function [score, p, q, c, sm]=getscoreOrg(dataref, dataplay, FAST, NumBar, SubSample, DurationRatio)

sm=1-simmx(dataref, dataplay);
if FAST
    [p, q, c]=dpfast(sm);
else
    [p, q, c]=dp2(sm);
end

score=c(size(c,1), size(c,2))/NumBar/SubSample/mean(mean(dataref))/DurationRatio;

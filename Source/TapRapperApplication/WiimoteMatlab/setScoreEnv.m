function [reference, starrange, SubSample, PlayWindow]=setScoreEnv(playtype, refpath)

switch playtype
    case 'FEET'
        SubSample=16;
        reference=[refpath, 'feet.mat'];
        %starrange=[1, 2, 4, 6, 100];
        %starrange=[2.3, 2.8, 3.3, 3.53, 100];
        starrange=[3, 3.8, 4.5, 6., 100];
    case 'TOESTAND'
        SubSample=64;
        reference=[refpath, 'toe_stand.mat'];
        %starrange=[0.9, 1.4, 1.55, 1.85, 100];
        starrange=[1.1, 1.6, 1.75, 1.85, 100];
    case 'POPOVERS'
        SubSample=64;
        reference=[refpath, 'pop_over.mat'];
        %starrange=[0.8, 1.35, 1.5, 1.8, 100];
        %starrange=[0.8, 1.15, 1.25, 1.8, 100]; %[5, 4, 3, 2, 1]
        starrange=[0.8, 1.45, 1.6, 1.8, 100]; %[5, 4, 3, 2, 1]
end

PlayWindow=64;
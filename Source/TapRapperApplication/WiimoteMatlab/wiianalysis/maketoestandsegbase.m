global subcost
subcost
i1=2; toestandsubcost(i1,:)=subcost

i1=4; weitoestand(i1,:)=subcost

toestandscore.annmarie=toestandsubcost
toestandscore.wei=weitoestand

toestandscore.annmariemean=mean(toestandscore.annmarie)
toestandscore.meanwei=mean(toestandscore.wei)

toestandscore.meanwei./toestandscore.annmariemean

cd D:\Projects\WiimoteProjects\WiiMotionPlus\Version29-WithTUIO\Data\matlab\scores
save toestandscore toestandscore

clear all
cd ..
load toe_stand
dataref.fname='toe_stand';
cd scores
load toestandscore

dataref.segmentbase=toestandscore.annmariemean;
dataref.segmark=[0.1079, 0.8918, 1.3576, 1.9256, 2.6073, 3.3003, 4.0615, 4.8681, 5.6179, 6.1291,...
            6.7540, 7.2538, 8.0264, 8.4126, 8.8103, 9.5146]*1000.;
cd ..
save toe_stand dataref
global subcost
subcost
i1=4; popoversubcost(i1,:)=subcost

i1=3; weipopover(i1,:)=subcost

popoverscore.annmarie=popoversubcost
popoverscore.wei=weipopover

popoverscore.annmariemean=mean(popoverscore.annmarie)
popoverscore.meanwei=mean(popoverscore.wei)

popoverscore.meanwei./popoverscore.annmariemean

cd D:\Projects\WiimoteProjects\WiiMotionPlus\Version29-WithTUIO\Data\matlab
save popoverscore popoverscore

clear all
load pop_over
dataref.fname='pop_over';
load popoverscore

dataref.segmentbase=popoverscore.annmariemean;
dataref.segmark=[0.3576, 1.4473, 2.6846, 3.8765, 5.0116, 6.2603, 7.5203, 8.6100, 9.5068]*1000.;

save pop_over dataref
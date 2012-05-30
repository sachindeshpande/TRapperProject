function rot=ypr2rot(yaw, pitch, roll)
%http://planning.cs.uiuc.edu/node103.html
%http://planning.cs.uiuc.edu/node102.html

MPitch=[1, 0, 0; 0, cos(pitch), sin(pitch); 0, -sin(pitch), cos(pitch)];
MRoll=[cos(roll), 0, -sin(roll); 0, 1, 0; sin(roll), 0, cos(roll)];
MYaw=[cos(yaw), sin(yaw), 0; -sin(yaw), cos(yaw), 0; 0, 0, 1];
rot=MYaw*MPitch*MRoll;

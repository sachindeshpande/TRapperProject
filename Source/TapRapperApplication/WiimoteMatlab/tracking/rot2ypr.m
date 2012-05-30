function [yaw, pitch, roll]=rot2ypr(rot)
pitch=-atan(rot(3,2)/sqrt(rot(3,1)^2+rot(3,3)^2)); %-Wx @ z/
yaw=atan2(rot(1,2),rot(2,2)); %WX @y / WX @x assume pitch < pi/2 is ture
roll=atan2(rot(3,1),rot(3,3));
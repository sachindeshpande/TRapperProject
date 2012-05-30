function data=normalizewiidata(data)
factora=1.; factorw=100.;
data.wii1.ax=data.wii1.ax/factora;
data.wii1.ay=data.wii1.ay/factora;
data.wii1.az=data.wii1.az/factora;
data.wii1.pitch=data.wii1.pitch/factorw;
data.wii1.roll=data.wii1.roll/factorw;
data.wii1.yaw=data.wii1.yaw/factorw;

data.wii2.ax=data.wii2.ax/factora;
data.wii2.ay=data.wii2.ay/factora;
data.wii2.az=data.wii2.az/factora;
data.wii2.pitch=data.wii2.pitch/factorw;
data.wii2.roll=data.wii2.roll/factorw;
data.wii2.yaw=data.wii2.yaw/factorw;


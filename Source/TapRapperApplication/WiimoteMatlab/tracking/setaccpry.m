function data=setaccpry(data)

data.wii1.accpitchcal=-atan((data.wii1.ay)./sqrt((data.wii1.ax).^2+(data.wii1.az).^2));
data.wii1.accrollcal=atan2((data.wii1.ax),(data.wii1.az));
data.wii1.accyawcal=0 * data.wii1.accroll;


data.wii2.accpitchcal=-atan((data.wii2.ay)./sqrt((data.wii2.ax).^2+(data.wii2.az).^2));
data.wii2.accrollcal=atan2((data.wii2.ax),(data.wii2.az));
data.wii2.accyawcal=0 * data.wii2.accroll;


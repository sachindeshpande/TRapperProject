size(wiimoteDataSave)

tt=wiimoteDataSave(:,2);

dtt=tt(2:end)-tt(1:end-1);
[x,y]=ginput(2)
range=round(x(1)):round(x(2));
plot(dtt(range))
mean(dtt(range))
(tt(end)-tt(1))/length(tt)

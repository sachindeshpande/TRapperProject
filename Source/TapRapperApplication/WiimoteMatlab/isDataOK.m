function status=isDataOK(data)
status=1;
if length(data.time)<100
    msgbox(['play data very short, length is ', num2str(length(data.time)), ', please check wiimote connection.']);
    status=0;
    return;
end

test=data.wii1.ax;
test=test-test(1);
if sum(abs(test))<0.0001
    msgbox('Left wiimote Acc droped connection!');
    status=-1;
    return;
end

test=data.wii2.ax;
test=test-test(1);
if sum(abs(test))<0.0001
    msgbox('Right wiimote Acc droped connection!');
    status=-2;
    return;
end

test=data.wii1.pitch;
test=test-test(1);
if sum(abs(test))<0.0001
    msgbox('Left wiimote Gyro droped connection!');
    status=-3;
    return;
end

test=data.wii2.pitch;
test=test-test(1);
if sum(abs(test))<0.0001
    msgbox('Right wiimote Gyro droped connection!');
    status=-4;
    return;
end

global wiimoteData wiimoteDataSave wii 

if(exist('t'))
    stop(t);
    delete(t);
    clear t wiimoteData
end

figure(1); clf;
t = timer;
   % Examine the variable every ten seconds
set(t, 'Period', 0.1);
set(t, 'ExecutionMode', 'FixedDelay');
% Set up the callback
%set(t, 'TimerFcn', 'mycallback()');
set(t, 'TimerFcn', 'trackingcallback()');
   % Start the timer
start(t)

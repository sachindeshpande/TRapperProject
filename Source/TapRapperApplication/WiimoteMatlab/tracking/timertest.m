t = timer;
   % Examine the variable every ten seconds
set(t, 'Period', 0.1);
set(t, 'ExecutionMode', 'FixedDelay');
% Set up the callback
set(t, 'TimerFcn', 'mycallback()');
   % Start the timer
start(t)
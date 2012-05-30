global score stars
%playtype='FEET';
%playtype='TOESTAND';
playtype='POPOVERS';

if not(exist('fpath'))
    fpath=pwd;
end
[fname, fpath]=uigetfile('*.csv', ['Open ', playtype, ' Data'], [fpath, '\a.csv']);
fnameplay=[fpath, fname];
playvsref(playtype,fnameplay)
%compareplayref






global score stars
playtype='FEET';
playtype='TOESTAND';
%playtype='POPOVERS';

switch playtype
    case 'FEET'
        str_filter='feet';
    case 'TOESTAND'
        str_filter='toe';
    case 'POPOVERS'
        str_filter='pop';
end



if not(exist('fpath'))
    fpath='D:\Projects\WiimoteProjects\data';
end
[fname, fpath]=uigetfile([fpath, '\*', str_filter,'*.csv'], ['Open ', playtype, ' Data'], [fpath, '\a.csv']);
fnameplay=[fpath, fname];

playvsref(playtype,fnameplay);

disp([round(score), stars]);

%compareplayref






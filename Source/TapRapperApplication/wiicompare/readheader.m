%[fname, fpath]=uigetfile('*.csv');
%fname=[fpath, fname];
%fid=fopen(fname);

data.LRSwitched=false;

a=fgetl(fid);
string=strread(a, '%s', 'delimiter', ',');

%while(not(isempty(string{1})))
while(not(strcmpi(string{1}, 'Calibration Info End')))
    %display('...');
    if strcmpi(string{1}, 'BPM')
        data.BPM=str2num(string{2});
    elseif strcmpi(string{1}, 'BPB')
        data.BPB=str2num(string{2});
    elseif strcmpi(string{1}, 'NumBar')
        data.NumBar=str2num(string{2});
    elseif strcmpi(string{1}, 'LeadIn')
        data.LeadIn=str2num(string{2});
    elseif strcmpi(string{1}, 'MusicFile')
        data.MusicFile=string{2};
    elseif strcmpi(string{1}, 'LRSwitched')
        if strcmpi(string{2}, 'TRUE')
            data.LRSwitched=true;
        end
    elseif strcmpi(string{1}, 'Sequence #')
        C=textscan(fid, '%d%d%f%f%f%f%f%s%s%s%f%f%f%f%f%f%f%f%s%s%s%f%f%f', 'Delimiter', ',' );
        data.wii1.cala=[C{5}, C{6}, C{7}];
        data.wii1.calw=[C{11}, C{12}, C{13}];
        data.wii2.cala=[C{16}, C{17}, C{18}];
        data.wii2.calw=[C{22}, C{23}, C{24}];
    end
    a=fgetl(fid);
        while(isempty(a))
            a=fgetl(fid);
        end
        
    string=strread(a, '%s', 'delimiter', ',');
end

for i1=1:2
    a=fgetl(fid);
    string=strread(a, '%s', 'delimiter', ',');
end

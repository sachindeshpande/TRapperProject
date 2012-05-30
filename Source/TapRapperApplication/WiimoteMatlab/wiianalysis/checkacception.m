playpower0=sum(dataplay.*dataplay);
%warp to ref time
playpower=0*playpower0;
for i1=1:length(p)
 playpower(p(i1))=playpower0(q(i1));
end

accepted=Pmask&(not(PScore));

figure(1); clf
plot(sqrt(refpower)-1, 'k'); hold on
%plot(sqrt(playpower0)-1, 'b');
plot(sqrt(playpower)-1, 'r');
plot(accepted*5, 'm');
plot(Pmask*5+6, 'b');
legend('ref', 'warped play', 'accepted', 'mask');

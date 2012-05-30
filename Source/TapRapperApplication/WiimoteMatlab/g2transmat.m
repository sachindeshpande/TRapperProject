function TransMat=g2transmat(a)
G=a/sqrt(sum(a.*a));

Y=[0, G(3), -G(2)];
Y=Y/sqrt(sum(Y.*Y));
X=cross(Y, G);

TransMat=[X', Y', G'];% [x, y, z] in columes

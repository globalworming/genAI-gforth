#! /usr/local/bin/gforth

require ../lib/test-more.fs
require ../reduce.fs

20 plan

t{

         0 nmin           t=
       1 1 nmin     1 =ok t=

 1  2  3 3 nmin     1 =ok t=
 3  1  2 3 nmin     1 =ok t=
 2  3  1 3 nmin     1 =ok t=
                      
 1  2  3 3 nmax     3 =ok t=
 3  1  2 3 nmax     3 =ok t=
 2  3  1 3 nmax     3 =ok t=

10 20 30 3 nsum    60 =ok t=
10 20 30 3 nprod 6000 =ok t=

t}

bye

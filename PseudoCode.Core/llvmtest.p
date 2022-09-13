DECLARE a, b : INTEGER
DECLARE r1, r2 : REAL
DECLARE c : __CHARPTR
a <- 1 + 2
b <- a
r1 <- 2.3
r2 <- r1 + a
r1 <- r2 + r1
r2 <- b + r1
c <- "hello world"
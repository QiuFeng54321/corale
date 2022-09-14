DECLARE i1, i2 : INTEGER
DECLARE r1, r2 : REAL
DECLARE c : __CHARPTR
i1 <- 1 + 2
i2 <- i1
r1 <- 2.3
r2 <- r1 + i1
b1 <- TRUE + 2
DECLARE b1, b2 : BOOLEAN
b2 <- FALSE
b1 <- b2 AND NOT b1 OR b2 OR i1 = i2
r1 <- r2 + r1
r2 <- i2 + r1
i2 <- r2 DIV 4
c <- "hello world"
c <- "abc"
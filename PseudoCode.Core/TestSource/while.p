DECLARE i : INTEGER
DECLARE res : INTEGER

i <- 1
res <- 1
WHILE i < 10
    res <- res * i
    i <- i + 1
ENDWHILE

OUTPUT i, res

i <- 1
res <- 1
REPEAT
    res <- res * i
    i <- i + 1
UNTIL i = 10

OUTPUT i, res
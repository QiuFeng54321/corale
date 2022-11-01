DECLARE i : INTEGER
DECLARE j : INTEGER
DECLARE res : INTEGER

//i <- 1
//res <- 1
//WHILE i < 10
//    res <- res * i
//    i <- i + 1
//ENDWHILE
//
//OUTPUT i, res
//
//i <- 1
//res <- 1
//REPEAT
//    res <- res * i
//    i <- i + 1
//UNTIL i = 10
//
//OUTPUT i, res

res <- 1
FOR i <- 1 TO 9
    FOR j <- 9 TO i STEP -1
        OUTPUT i, "*", j, "=", i * j
    NEXT j
    res <- res * i
NEXT i

OUTPUT i, res
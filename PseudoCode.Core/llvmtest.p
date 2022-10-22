DECLARE i1, i2 : INTEGER
DECLARE r1, r2 : REAL
DECLARE b1, b2 : BOOLEAN
DECLARE c : ^CHAR
FUNCTION recFib(n : INTEGER) RETURNS INTEGER
    IF n = 1 OR n = 2 THEN
        RETURN 1
    ENDIF
    RETURN recFib(n - 1) + recFib(n - 2)
ENDFUNCTION
FUNCTION swap(BYREF a : INTEGER, b : INTEGER) RETURNS VOID
    DECLARE tmp : INTEGER
    tmp <- a
    a <- b
    b <- tmp
ENDFUNCTION
i1 <- 1 + 2
i2 <- 4
CALL swap(i1, i2)
CALL __PRINTF("\nHello\n")
CALL __PRINTF(i1)
CALL __PRINTF(i2)
CALL __PRINTF(RAND(100))
CALL __PRINTF(TO_UPPER(__STR("喵adFSEf")))
CALL __PRINTLN()
r1 <- 2.3
r2 <- r1 + i1
b1 <- TRUE + 2
b2 <- FALSE
CALL __PRINTF(b1)
CALL __PRINTF(recFib(10))
IF FALSE THEN
    CALL __PRINTF('c')
ELSE
    IF FALSE THEN
        CALL __PRINTF('e')
    ELSE
        CALL __PRINTF('f')
    ENDIF
    CALL __PRINTF('d')
ENDIF
TYPE t1<T, S>
    DECLARE t1t : T
    DECLARE s : INTEGER
    DECLARE t1s : S
ENDTYPE
TYPE t2<T, S>
    DECLARE t2t1 : t1<S, T>
    DECLARE t2t2 : t1<T, S>
ENDTYPE
DECLARE t11 : t1<INTEGER, BOOLEAN>
DECLARE t12 : t1<^CHAR, t1<INTEGER, REAL>>
DECLARE t21 : t2<INTEGER, ^^CHAR>
b1 <- b2 AND(NOT(b1 OR b2)) OR i1= - i2 // Testing unformatted string
r1 <- r2 + r1
r2 <- i2 + r1
i2 <- r2 DIV 4
c <- "hello world"
c <- "abc"
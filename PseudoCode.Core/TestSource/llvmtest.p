DECLARE i1, i2 : INTEGER
DECLARE r1, r2 : REAL
DECLARE b1, b2 : BOOLEAN
DECLARE c : STRING
FUNCTION swap<T>(BYREF a : T, b : T) RETURNS VOID
    DECLARE tmp : T
    tmp <- a
    a <- b
    b <- tmp
ENDFUNCTION
FUNCTION recFib(n : INTEGER) RETURNS INTEGER
    IF n = 1 OR n = 2 THEN
        RETURN 1
    ENDIF
    RETURN recFib(n - 1) + recFib(n - 2)
ENDFUNCTION
i1 <- 1 + 2
INPUT i2
CALL swap<INTEGER>(i1, i2)
CALL swap<INTEGER>(i1, i2)
OUTPUT "Hello", i1, i2, RAND(100)
c <- "hello world"
OUTPUT (c.Pointer + 9) - (c.Pointer + 2), TO_UPPER("啊adFSEf"), MID("fa哈哈哈", 2, 3)
r1 <- 2.3
r2 <- r1 + i1
b1 <- TRUE
b2 <- FALSE
OUTPUT b1, recFib(10)
IF FALSE THEN
    OUTPUT 'c'
ELSE
    IF FALSE THEN
        OUTPUT 'e'
    ELSE
        OUTPUT 'f'
    ENDIF
    OUTPUT 'd' > 'a'
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
t11.t1t <- 3
OUTPUT t11.t1t
DECLARE t12 : t1<^CHAR, t1<INTEGER, REAL>>
DECLARE t13 : t1<^CHAR, t1<INTEGER, REAL>>
DECLARE t21 : t2<INTEGER, ^^CHAR>
b1 <- b2 AND(NOT(b1 OR b2)) OR i1= - i2 // Testing unformatted string
r1 <- r2 + r1
r2 <- i2 + r1
OUTPUT r1, r2
CALL swap<REAL>(r1, r2)
OUTPUT r1, r2

i2 <- r2 DIV 4
c <- "abc"
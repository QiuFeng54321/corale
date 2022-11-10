DECLARE freq : ARRAY[0:1000] OF INTEGER
DECLARE n : INTEGER
DECLARE i : INTEGER
DECLARE j : INTEGER
DECLARE num : INTEGER

FOR i <- 1 TO 1000
    freq[i] <- 0
NEXT i
INPUT n
FOR i <- 1 TO n
    INPUT num
    freq[num] <- freq[num] + 1
NEXT i

FOR i <- 0 TO 1000
    FOR j <- 1 TO freq[i]
        OUTPUT i
    NEXT j
NEXT i

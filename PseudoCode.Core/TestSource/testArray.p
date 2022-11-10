DECLARE arr : INTEGER[3]
arr <- [2, 3, 4]
DECLARE len : INTEGER
len <- 3
TYPE TestArrType
    DECLARE arr : ^^INTEGER
    DECLARE i : INTEGER
ENDTYPE

DECLARE testVal : TestArrType
testVal.i <- 3
DECLARE arr1d : ^INTEGER
DECLARE l : INTEGER
arr1d <- [1, 2, 3] 
OUTPUT (arr1d + 1)^


DECLARE arr2d : ^^INTEGER
DECLARE i : INTEGER
arr2d <- MALLOC 3 FOR ^INTEGER
FOR i <- 0 TO 2
    (arr2d + i)^ <- [i, i * 2, i * 3]
NEXT i
OUTPUT ((arr2d + 1)^ + 2)^

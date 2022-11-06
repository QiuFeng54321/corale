DECLARE arr : INTEGER[3]
arr <- [2, 3, 4]
DECLARE len : INTEGER
len <- 10

TYPE testArrType
    DECLARE arr : INTEGER[][]
ENDTYPE

DECLARE testVal : testArrType
testVal.arr <- [[3, 4, 5], [4, 3, 2], [3, 4, 9]]
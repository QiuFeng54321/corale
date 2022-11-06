DECLARE arr : INTEGER[3]
arr <- [2, 3, 4]
DECLARE len : INTEGER
len <- 10

TYPE TestArrType
    DECLARE arr : ^^INTEGER
ENDTYPE

DECLARE testVal : TestArrType
testVal.arr <- [[3, 4], [4, 3], [3, 4]]
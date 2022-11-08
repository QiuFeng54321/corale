// DECLARE arr : INTEGER[3]
// arr <- [2, 3, 4]
// DECLARE len : INTEGER
// len <- 10

// TYPE TestArrType
//     DECLARE arr : ^^INTEGER
//     DECLARE i : INTEGER
// ENDTYPE
// 
// DECLARE testVal : TestArrType
// testVal.i <- 3
DECLARE arr1d : ^INTEGER
DECLARE l : INTEGER
arr1d <- [1, 2, 3] 
OUTPUT (arr1d + 1)^


DECLARE arr2d : ^^INTEGER
arr2d <- MALLOC 3 FOR ^INTEGER
FOR l <- 0 TO 2
    (arr2d + l)^ <- [1, 2, 3]
NEXT l
OUTPUT ((arr2d + 1)^ + 1)^

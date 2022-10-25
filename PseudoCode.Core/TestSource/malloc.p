DECLARE intSize : INTEGER
intSize <- SIZEOF INTEGER
OUTPUT intSize
DECLARE intArrPtr : ^INTEGER
intArrPtr <- MALLOC 5 FOR INTEGER
(intArrPtr + 2)^ <- 3
OUTPUT (intArrPtr + 2)^
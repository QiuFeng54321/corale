DECLARE tySize : INTEGER
TYPE t1<T, S>
    DECLARE t1t : T
    DECLARE s : INTEGER
    DECLARE t1s : S
ENDTYPE
tySize <- SIZEOF t1<INTEGER, STRING>
OUTPUT tySize
DECLARE intArrPtr : ^t1<INTEGER, STRING>
intArrPtr <- MALLOC 5 FOR t1<INTEGER, STRING>
(intArrPtr + 1)^.s <- 3
OUTPUT (intArrPtr + 1)^.s

TYPE Bounds
    DECLARE Lower, Upper : INTEGER
ENDTYPE
TYPE __ARRAY<T>
    DECLARE ElementPtr : ^T
    DECLARE Dimensions : ^Bounds
    DECLARE DimensionCount : INTEGER
ENDTYPE
FUNCTION Make1DArray<T>(count : INTEGER) RETURNS __ARRAY<T>
    DECLARE res : __ARRAY<T>
    res.ElementPtr <- MALLOC count FOR T
    res.Dimensions <- MALLOC 1 FOR Bounds
    res.DimensionCount <- 1
    res.Dimensions^.Lower <- 0
    res.Dimensions^.Upper <- count - 1
    RETURN res
ENDFUNCTION
FUNCTION Get1DArrayLength<T>(arr : __ARRAY<T>) RETURNS INTEGER
    RETURN arr.Dimensions^.Upper - arr.Dimensions^.Lower + 1
ENDFUNCTION
FUNCTION ElementAt<T>(arr : __ARRAY<T>, index : INTEGER) RETURNS BYREF T
    RETURN (arr.ElementPtr + index - arr.Dimensions^.Lower)^
ENDFUNCTION
OPERATOR Add(arr : __ARRAY<INTEGER>, index : INTEGER) RETURNS BYREF INTEGER
    RETURN ElementAt<INTEGER>(arr, index)
ENDFUNCTION
OPERATOR Subtract(arr : __ARRAY<INTEGER>, index : INTEGER) RETURNS BYREF INTEGER
    RETURN ElementAt<INTEGER>(arr, Get1DArrayLength<INTEGER>(arr) - index + arr.Dimensions^.Lower + 1)
ENDFUNCTION
DECLARE arr : __ARRAY<INTEGER>
arr <- Make1DArray<INTEGER>(5)
arr + 2 <- 3
OUTPUT arr - 4
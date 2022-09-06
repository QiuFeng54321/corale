EXTERN FUNCTION __malloc<T>(count : INTEGER) RETURNS ^T
CLASS RANGE
    PUBLIC Start, End : INTEGER
    PUBLIC FUNCTION Length RETURNS INTEGER
        RETURN End - Start + 1
    ENDFUNCTION
    PUBLIC FUNCTION GetZeroIndex(i : INTEGER) RETURNS INTEGER
        RETURN i - Start
    ENDFUNCTION
ENDCLASS

// Actual array
// Uses Generic (Actually template), Extern functions, pointers, Custom operators
CLASS ARRAY<T>
    Internal : ^T
    Dimensions : ^RANGE
    Length : INTEGER
    PUBLIC PROCEDURE NEW(dimensions : ^RANGE, dimCount : INTEGER)
        DECLARE i : INTEGER
        Dimensions <- dimensions
        Total <- 0
        FOR i <- 0 TO dimCount - 1
            Total <- Total + Dimensions[i]
        NEXT i
        Internal <- __malloc<T>(Total)
    ENDPROCEDURE
    PUBLIC FUNCTION OperatorArrayAccess(indices : ^RANGE, dimCount : INTEGER) RETURNS T
        DECLARE resIndex : INTEGER
        DECLARE i : INTEGER
        DECLARE size : INTEGER
        resIndex <- 0
        size <- 1
        FOR i <- dimCount - 1 TO 0 STEP -1
            resIndex <- resIndex + dimensions[i].GetZeroIndex(indices[i]) * size
            size <- size * dimensions[i].Length()
        NEXT i
        RETURN (Internal + resIndex)^
    ENDFUNCTION
ENDCLASS
            
        
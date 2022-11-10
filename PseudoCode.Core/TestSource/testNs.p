NAMESPACE algorithms::math
    FUNCTION recFib<T>(n : T) RETURNS T
        IF n = 1 OR n = 2 THEN
            RETURN 1
            // n <- 1
        ENDIF
        RETURN recFib(n - 1) + recFib(n - 2)
    ENDFUNCTION
ENDNAMESPACE
USE NAMESPACE algorithms
OUTPUT math::recFib<INTEGER>(30)
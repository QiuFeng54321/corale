
FUNCTION recFib(n : INTEGER) RETURNS INTEGER
    IF n = 1 OR n = 2 THEN
        RETURN 1
    ENDIF
    RETURN recFib(n - 1) + recFib(n - 2)
ENDFUNCTION
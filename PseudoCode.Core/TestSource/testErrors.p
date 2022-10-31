// IMPORT "malloc.p"
IMPORT "testModue.p"
DECLARE i : REAL
426 <- RANDr(100)
OUTPUT i
IF i >= 49 THEN
    OUTPUT testModulve::recFib(20)
ELSE
    OUTPUT testModule::rechFib(10)
ENDIF
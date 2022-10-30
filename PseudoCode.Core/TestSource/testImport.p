// IMPORT "malloc.p"
IMPORT "testModule.p"
IMPORT "testModule.p"
DECLARE i : REAL
i3 <- RAND(100)
OUTPUT i
IF i >= 49 THEN
    OUTPUT testModule::recFib(20)
ELSE
    OUTPUT testModule::recFib(10)
ENDIF

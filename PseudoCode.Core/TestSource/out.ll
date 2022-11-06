; ModuleID = 'testArray'
source_filename = "testArray"
target datalayout = "e-m:o-p270:32:32-p271:32:32-p272:64:64-i64:64-f80:128-n8:16:32:64-S128"

define void @testArray() #0 {
entry:
  %arr = alloca [3 x i64], align 8
  store [3 x i64] [i64 2, i64 3, i64 4], [3 x i64]* %arr, align 8
  %len = alloca i64, align 8
  store i64 10, i64* %len, align 8
  %_load.0 = load i64, i64* %len, align 8
  %testVal = alloca <{ [3 x i64*] }>, align 8
  %_arr.0 = getelementptr inbounds <{ [3 x i64*] }>, <{ [3 x i64*] }>* %testVal, i32 0, i32 0
  store [3 x [2 x i64]] [[2 x i64] [i64 3, i64 4], [2 x i64] [i64 4, i64 3], [2 x i64] [i64 3, i64 4]], [3 x i64*]* %_arr.0, align 8
  ret void
}

declare <{ i8*, i64 }> @RIGHT(<{ i8*, i64 }>, i64)

declare <{ i8*, i64 }> @LEFT(<{ i8*, i64 }>, i64)

declare i64 @LENGTH(<{ i8*, i64 }>)

declare i1 @__in_range(double, double, double)

declare <{ i8*, i64 }> @MID(<{ i8*, i64 }>, i64, i64)

declare i8 @LCASE(i8)

declare i8 @UCASE(i8)

declare <{ i8*, i64 }> @TO_UPPER(<{ i8*, i64 }>)

declare <{ i8*, i64 }> @TO_LOWER(<{ i8*, i64 }>)

declare <{ i8*, i64 }> @NUM_TO_STR(double)

declare double @STR_TO_NUM(<{ i8*, i64 }>)

declare i1 @IS_NUM(<{ i8*, i64 }>)

declare i64 @ASC(i8)

declare i8 @CHR(i64)

declare i64 @DAY(<{ i64, i64, i64 }>)

declare i64 @MONTH(<{ i64, i64, i64 }>)

declare i64 @YEAR(<{ i64, i64, i64 }>)

declare <{ i64, i64, i64 }> @SETDATE(i64, i64, i64)

declare i64 @INT(double)

declare i64 @FIND(<{ i8*, i64 }>, i8)

declare double @RAND(i64)

declare <{ i8*, i64 }> @__STR(i8*)

declare void @__PRINTF(i64)

declare void @__PRINTF.1(i8)

declare void @__PRINTF.2(double)

declare void @__PRINTF.3(i1)

declare void @__PRINTF.4(<{ i8*, i64 }>)

declare void @__PRINTF.5(i8*)

declare void @__PRINTF.6(<{ i64, i64, i64 }>)

declare void @__PRINTLN()

declare void @__SCAN(i64*)

declare void @__SCAN.7(double*)

declare void @__SCAN.8(i1*)

declare void @__SCAN.9(i8*)

declare void @__SCAN.10(<{ i8*, i64 }>*)

declare void @__SCAN.11(<{ i64, i64, i64 }>*)

attributes #0 = { "frame-pointer"="none" }

!llvm.dbg.cu = !{!0}

!0 = distinct !DICompileUnit(language: DW_LANG_C, file: !1, producer: "PseudoCode", isOptimized: false, runtimeVersion: 0, emissionKind: FullDebug, splitDebugInlining: false)
!1 = !DIFile(filename: "testArray.p", directory: "")

; ModuleID = 'testMalloc'
source_filename = "testMalloc"
target datalayout = "e-m:o-p270:32:32-p271:32:32-p272:64:64-i64:64-f80:128-n8:16:32:64-S128"

define void @testMalloc() #0 {
entry:
  %tySize = alloca i64, align 8
  store i64 ptrtoint (<{ i64, i64, <{ i8*, i64 }> }>* getelementptr (<{ i64, i64, <{ i8*, i64 }> }>, <{ i64, i64, <{ i8*, i64 }> }>* null, i32 1) to i64), i64* %tySize, align 8
  %_load.0 = load i64, i64* %tySize, align 8
  call void @__PRINTF(i64 %_load.0)
  call void @__PRINTF.1(i8 10)
  %intArrPtr = alloca <{ i64, i64, <{ i8*, i64 }> }>*, align 8
  %0 = trunc i64 5 to i32
  %mallocsize = mul i32 %0, ptrtoint (<{ i64, i64, <{ i8*, i64 }> }>* getelementptr (<{ i64, i64, <{ i8*, i64 }> }>, <{ i64, i64, <{ i8*, i64 }> }>* null, i32 1) to i32)
  %malloccall = tail call i8* @malloc(i32 %mallocsize)
  %malloc.0 = bitcast i8* %malloccall to <{ i64, i64, <{ i8*, i64 }> }>*
  store <{ i64, i64, <{ i8*, i64 }> }>* %malloc.0, <{ i64, i64, <{ i8*, i64 }> }>** %intArrPtr, align 8
  %_load.1 = load <{ i64, i64, <{ i8*, i64 }> }>*, <{ i64, i64, <{ i8*, i64 }> }>** %intArrPtr, align 8
  %_int.0 = getelementptr inbounds <{ i64, i64, <{ i8*, i64 }> }>, <{ i64, i64, <{ i8*, i64 }> }>* %_load.1, i64 1
  %_s.0 = getelementptr inbounds <{ i64, i64, <{ i8*, i64 }> }>, <{ i64, i64, <{ i8*, i64 }> }>* %_int.0, i32 0, i32 1
  store i64 3, i64* %_s.0, align 8
  %_load.2 = load <{ i64, i64, <{ i8*, i64 }> }>*, <{ i64, i64, <{ i8*, i64 }> }>** %intArrPtr, align 8
  %_int.1 = getelementptr inbounds <{ i64, i64, <{ i8*, i64 }> }>, <{ i64, i64, <{ i8*, i64 }> }>* %_load.2, i64 1
  %_s.1 = getelementptr inbounds <{ i64, i64, <{ i8*, i64 }> }>, <{ i64, i64, <{ i8*, i64 }> }>* %_int.1, i32 0, i32 1
  %_load.3 = load i64, i64* %_s.1, align 8
  call void @__PRINTF(i64 %_load.3)
  call void @__PRINTF.1(i8 10)
  %arr = alloca <{ i64*, <{ i64, i64 }>*, i64 }>, align 8
  %_type.0 = call <{ i64*, <{ i64, i64 }>*, i64 }> @.testMalloc.Make1DArray(i64 5)
  store <{ i64*, <{ i64, i64 }>*, i64 }> %_type.0, <{ i64*, <{ i64, i64 }>*, i64 }>* %arr, align 1
  %_load.19 = load <{ i64*, <{ i64, i64 }>*, i64 }>, <{ i64*, <{ i64, i64 }>*, i64 }>* %arr, align 1
  %_int.13 = call i64* @.testMalloc.Add(<{ i64*, <{ i64, i64 }>*, i64 }> %_load.19, i64 2)
  store i64 3, i64* %_int.13, align 8
  %_load.20 = load <{ i64*, <{ i64, i64 }>*, i64 }>, <{ i64*, <{ i64, i64 }>*, i64 }>* %arr, align 1
  %_int.14 = call i64* @.testMalloc.Subtract(<{ i64*, <{ i64, i64 }>*, i64 }> %_load.20, i64 4)
  %_load.21 = load i64, i64* %_int.14, align 8
  call void @__PRINTF(i64 %_load.21)
  call void @__PRINTF.1(i8 10)
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

declare noalias i8* @malloc(i32)

define weak i64* @.testMalloc.Add(<{ i64*, <{ i64, i64 }>*, i64 }> %arr, i64 %index) {
entry:
  %0 = alloca <{ i64*, <{ i64, i64 }>*, i64 }>, align 8
  store <{ i64*, <{ i64, i64 }>*, i64 }> %arr, <{ i64*, <{ i64, i64 }>*, i64 }>* %0, align 1
  %_load.7 = load <{ i64*, <{ i64, i64 }>*, i64 }>, <{ i64*, <{ i64, i64 }>*, i64 }>* %0, align 1
  %_int.4 = call i64* @.testMalloc.Add.0.ElementAt(<{ i64*, <{ i64, i64 }>*, i64 }> %_load.7, i64 %index)
  ret i64* %_int.4
}

define weak i64* @.testMalloc.Add.0.ElementAt(<{ i64*, <{ i64, i64 }>*, i64 }> %arr, i64 %index) {
entry:
  %0 = alloca <{ i64*, <{ i64, i64 }>*, i64 }>, align 8
  store <{ i64*, <{ i64, i64 }>*, i64 }> %arr, <{ i64*, <{ i64, i64 }>*, i64 }>* %0, align 1
  %_ElementPtr.0 = getelementptr inbounds <{ i64*, <{ i64, i64 }>*, i64 }>, <{ i64*, <{ i64, i64 }>*, i64 }>* %0, i32 0, i32 0
  %_Dimensions.0 = getelementptr inbounds <{ i64*, <{ i64, i64 }>*, i64 }>, <{ i64*, <{ i64, i64 }>*, i64 }>* %0, i32 0, i32 1
  %_load.4 = load <{ i64, i64 }>*, <{ i64, i64 }>** %_Dimensions.0, align 8
  %_Lower.0 = getelementptr inbounds <{ i64, i64 }>, <{ i64, i64 }>* %_load.4, i32 0, i32 0
  %_load.5 = load i64, i64* %_Lower.0, align 8
  %_int.2 = sub i64 %index, %_load.5
  %_load.6 = load i64*, i64** %_ElementPtr.0, align 8
  %_int.3 = getelementptr inbounds i64, i64* %_load.6, i64 %_int.2
  ret i64* %_int.3
}

define weak i64* @.testMalloc.Subtract(<{ i64*, <{ i64, i64 }>*, i64 }> %arr, i64 %index) {
entry:
  %0 = alloca <{ i64*, <{ i64, i64 }>*, i64 }>, align 8
  store <{ i64*, <{ i64, i64 }>*, i64 }> %arr, <{ i64*, <{ i64, i64 }>*, i64 }>* %0, align 1
  %_load.12 = load <{ i64*, <{ i64, i64 }>*, i64 }>, <{ i64*, <{ i64, i64 }>*, i64 }>* %0, align 1
  %_int.7 = call i64 @.testMalloc.Subtract.0.Get1DArrayLength(<{ i64*, <{ i64, i64 }>*, i64 }> %_load.12)
  %_int.8 = sub i64 %_int.7, %index
  %_Dimensions.3 = getelementptr inbounds <{ i64*, <{ i64, i64 }>*, i64 }>, <{ i64*, <{ i64, i64 }>*, i64 }>* %0, i32 0, i32 1
  %_load.13 = load <{ i64, i64 }>*, <{ i64, i64 }>** %_Dimensions.3, align 8
  %_Lower.2 = getelementptr inbounds <{ i64, i64 }>, <{ i64, i64 }>* %_load.13, i32 0, i32 0
  %_load.14 = load i64, i64* %_Lower.2, align 8
  %_int.9 = add i64 %_int.8, %_load.14
  %_int.10 = add i64 %_int.9, 1
  %_load.15 = load <{ i64*, <{ i64, i64 }>*, i64 }>, <{ i64*, <{ i64, i64 }>*, i64 }>* %0, align 1
  %_int.11 = call i64* @.testMalloc.Add.0.ElementAt(<{ i64*, <{ i64, i64 }>*, i64 }> %_load.15, i64 %_int.10)
  ret i64* %_int.11
}

define weak i64 @.testMalloc.Subtract.0.Get1DArrayLength(<{ i64*, <{ i64, i64 }>*, i64 }> %arr) {
entry:
  %0 = alloca <{ i64*, <{ i64, i64 }>*, i64 }>, align 8
  store <{ i64*, <{ i64, i64 }>*, i64 }> %arr, <{ i64*, <{ i64, i64 }>*, i64 }>* %0, align 1
  %_Dimensions.1 = getelementptr inbounds <{ i64*, <{ i64, i64 }>*, i64 }>, <{ i64*, <{ i64, i64 }>*, i64 }>* %0, i32 0, i32 1
  %_load.8 = load <{ i64, i64 }>*, <{ i64, i64 }>** %_Dimensions.1, align 8
  %_Upper.0 = getelementptr inbounds <{ i64, i64 }>, <{ i64, i64 }>* %_load.8, i32 0, i32 1
  %_Dimensions.2 = getelementptr inbounds <{ i64*, <{ i64, i64 }>*, i64 }>, <{ i64*, <{ i64, i64 }>*, i64 }>* %0, i32 0, i32 1
  %_load.9 = load <{ i64, i64 }>*, <{ i64, i64 }>** %_Dimensions.2, align 8
  %_Lower.1 = getelementptr inbounds <{ i64, i64 }>, <{ i64, i64 }>* %_load.9, i32 0, i32 0
  %_load.10 = load i64, i64* %_Upper.0, align 8
  %_load.11 = load i64, i64* %_Lower.1, align 8
  %_int.5 = sub i64 %_load.10, %_load.11
  %_int.6 = add i64 %_int.5, 1
  ret i64 %_int.6
}

define weak <{ i64*, <{ i64, i64 }>*, i64 }> @.testMalloc.Make1DArray(i64 %count) {
entry:
  %res = alloca <{ i64*, <{ i64, i64 }>*, i64 }>, align 8
  %0 = trunc i64 %count to i32
  %mallocsize = mul i32 %0, ptrtoint (i64* getelementptr (i64, i64* null, i32 1) to i32)
  %malloccall = tail call i8* @malloc(i32 %mallocsize)
  %malloc.1 = bitcast i8* %malloccall to i64*
  %_ElementPtr.1 = getelementptr inbounds <{ i64*, <{ i64, i64 }>*, i64 }>, <{ i64*, <{ i64, i64 }>*, i64 }>* %res, i32 0, i32 0
  store i64* %malloc.1, i64** %_ElementPtr.1, align 8
  %1 = trunc i64 1 to i32
  %mallocsize1 = mul i32 %1, ptrtoint (<{ i64, i64 }>* getelementptr (<{ i64, i64 }>, <{ i64, i64 }>* null, i32 1) to i32)
  %malloccall2 = tail call i8* @malloc(i32 %mallocsize1)
  %malloc.2 = bitcast i8* %malloccall2 to <{ i64, i64 }>*
  %_Dimensions.4 = getelementptr inbounds <{ i64*, <{ i64, i64 }>*, i64 }>, <{ i64*, <{ i64, i64 }>*, i64 }>* %res, i32 0, i32 1
  store <{ i64, i64 }>* %malloc.2, <{ i64, i64 }>** %_Dimensions.4, align 8
  %_DimensionCount.0 = getelementptr inbounds <{ i64*, <{ i64, i64 }>*, i64 }>, <{ i64*, <{ i64, i64 }>*, i64 }>* %res, i32 0, i32 2
  store i64 1, i64* %_DimensionCount.0, align 8
  %_Dimensions.5 = getelementptr inbounds <{ i64*, <{ i64, i64 }>*, i64 }>, <{ i64*, <{ i64, i64 }>*, i64 }>* %res, i32 0, i32 1
  %_load.16 = load <{ i64, i64 }>*, <{ i64, i64 }>** %_Dimensions.5, align 8
  %_Lower.3 = getelementptr inbounds <{ i64, i64 }>, <{ i64, i64 }>* %_load.16, i32 0, i32 0
  store i64 0, i64* %_Lower.3, align 8
  %_int.12 = sub i64 %count, 1
  %_Dimensions.6 = getelementptr inbounds <{ i64*, <{ i64, i64 }>*, i64 }>, <{ i64*, <{ i64, i64 }>*, i64 }>* %res, i32 0, i32 1
  %_load.17 = load <{ i64, i64 }>*, <{ i64, i64 }>** %_Dimensions.6, align 8
  %_Upper.1 = getelementptr inbounds <{ i64, i64 }>, <{ i64, i64 }>* %_load.17, i32 0, i32 1
  store i64 %_int.12, i64* %_Upper.1, align 8
  %_load.18 = load <{ i64*, <{ i64, i64 }>*, i64 }>, <{ i64*, <{ i64, i64 }>*, i64 }>* %res, align 1
  ret <{ i64*, <{ i64, i64 }>*, i64 }> %_load.18
}

attributes #0 = { "frame-pointer"="none" }

!llvm.dbg.cu = !{!0}

!0 = distinct !DICompileUnit(language: DW_LANG_C, file: !1, producer: "PseudoCode", isOptimized: false, runtimeVersion: 0, emissionKind: FullDebug, splitDebugInlining: false)
!1 = !DIFile(filename: "testMalloc.p", directory: "")

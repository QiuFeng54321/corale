; ModuleID = 'testArray'
source_filename = "testArray"
target datalayout = "e-m:o-p270:32:32-p271:32:32-p272:64:64-i64:64-f80:128-n8:16:32:64-S128"

define void @testArray() #0 {
entry:
  %arr1d = alloca i64*, align 8
  %l = alloca i64, align 8
  %0 = alloca [3 x i64], align 8
  store [3 x i64] [i64 1, i64 2, i64 3], [3 x i64]* %0, align 8
  %1 = bitcast [3 x i64]* %0 to i64*
  store i64* %1, i64** %arr1d, align 8
  %_load.0 = load i64*, i64** %arr1d, align 8
  %_int.0 = getelementptr inbounds i64, i64* %_load.0, i64 1
  %2 = getelementptr inbounds i64, i64* %_int.0, i64 0
  %_load.1 = load i64, i64* %2, align 8
  call void @__PRINTF(i64 %_load.1)
  call void @__PRINTF.1(i8 10)
  %arr2d = alloca i64**, align 8
  %3 = trunc i64 3 to i32
  %mallocsize = mul i32 %3, ptrtoint (i64** getelementptr (i64*, i64** null, i32 1) to i32)
  %malloccall = tail call i8* @malloc(i32 %mallocsize)
  %malloc.0 = bitcast i8* %malloccall to i64**
  store i64** %malloc.0, i64*** %arr2d, align 8
  store i64 0, i64* %l, align 8
  br label %condition

condition:                                        ; preds = %entry
  %_load.2 = load i64, i64* %l, align 8
  %_bool.0 = icmp sgt i64 %_load.2, 2
  %_bool.1 = and i1 true, %_bool.0
  %_load.3 = load i64, i64* %l, align 8
  %_bool.2 = icmp slt i64 %_load.3, 2
  %_bool.3 = and i1 false, %_bool.2
  %_bool.4 = or i1 %_bool.1, %_bool.3
  br i1 %_bool.4, label %continue, label %block

continue:                                         ; preds = %block, %condition
  %_load.8 = load i64**, i64*** %arr2d, align 8
  %_int.3 = getelementptr inbounds i64*, i64** %_load.8, i64 1
  %4 = getelementptr inbounds i64*, i64** %_int.3, i64 0
  %_load.9 = load i64*, i64** %4, align 8
  %_int.4 = getelementptr inbounds i64, i64* %_load.9, i64 1
  %5 = getelementptr inbounds i64, i64* %_int.4, i64 0
  %_load.10 = load i64, i64* %5, align 8
  call void @__PRINTF(i64 %_load.10)
  call void @__PRINTF.1(i8 10)
  ret void

block:                                            ; preds = %increment, %condition
  %_load.4 = load i64**, i64*** %arr2d, align 8
  %_load.5 = load i64, i64* %l, align 8
  %_int.1 = getelementptr inbounds i64*, i64** %_load.4, i64 %_load.5
  %6 = getelementptr inbounds i64*, i64** %_int.1, i64 0
  %7 = alloca [3 x i64], align 8
  store [3 x i64] [i64 1, i64 2, i64 3], [3 x i64]* %7, align 8
  %8 = bitcast [3 x i64]* %7 to i64*
  store i64* %8, i64** %6, align 8
  %_load.6 = load i64, i64* %l, align 8
  %_bool.5 = icmp eq i64 %_load.6, 2
  br i1 %_bool.5, label %continue, label %increment

increment:                                        ; preds = %block
  %_load.7 = load i64, i64* %l, align 8
  %_int.2 = add i64 %_load.7, 1
  store i64 %_int.2, i64* %l, align 8
  br label %block
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

declare i64 @INSTR(i64, <{ i8*, i64 }>, i8)

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

attributes #0 = { "frame-pointer"="none" }

!llvm.dbg.cu = !{!0}

!0 = distinct !DICompileUnit(language: DW_LANG_C, file: !1, producer: "PseudoCode", isOptimized: false, runtimeVersion: 0, emissionKind: FullDebug, splitDebugInlining: false)
!1 = !DIFile(filename: "testArray.p", directory: "")

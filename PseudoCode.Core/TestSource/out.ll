; ModuleID = 'llvmtest'
source_filename = "llvmtest"
target datalayout = "e-m:o-p270:32:32-p271:32:32-p272:64:64-i64:64-f80:128-n8:16:32:64-S128"

@str.0 = private unnamed_addr constant [6 x i8] c"Hello\00", align 1
@str.1 = private unnamed_addr constant [12 x i8] c"hello world\00", align 1
@str.2 = private unnamed_addr constant [10 x i8] c"\E5\95\8AadFSEf\00", align 1
@str.3 = private unnamed_addr constant [12 x i8] c"fa\E5\93\88\E5\93\88\E5\93\88\00", align 1
@str.4 = private unnamed_addr constant [4 x i8] c"abc\00", align 1

define void @llvmtest() #0 {
entry:
  %i1 = alloca i64, align 8
  %i2 = alloca i64, align 8
  %r1 = alloca double, align 8
  %r2 = alloca double, align 8
  %b1 = alloca i1, align 1
  %b2 = alloca i1, align 1
  %c = alloca <{ i8*, i64 }>, align 8
  store i64 3, i64* %i1, align 8
  call void @__SCAN(i64* %i2)
  call void @.llvmtest.swap(i64* %i1, i64* %i2)
  call void @.llvmtest.swap(i64* %i1, i64* %i2)
  call void @__PRINTF.4(<{ i8*, i64 }> <{ i8* getelementptr inbounds ([6 x i8], [6 x i8]* @str.0, i32 0, i32 0), i64 5 }>)
  call void @__PRINTF.1(i8 32)
  %_load.3 = load i64, i64* %i1, align 8
  call void @__PRINTF(i64 %_load.3)
  call void @__PRINTF.1(i8 32)
  %_load.4 = load i64, i64* %i2, align 8
  call void @__PRINTF(i64 %_load.4)
  call void @__PRINTF.1(i8 32)
  %_real.0 = call double @RAND(i64 100)
  call void @__PRINTF.2(double %_real.0)
  call void @__PRINTF.1(i8 10)
  store <{ i8*, i64 }> <{ i8* getelementptr inbounds ([12 x i8], [12 x i8]* @str.1, i32 0, i32 0), i64 11 }>, <{ i8*, i64 }>* %c, align 1
  %_Pointer.0 = getelementptr inbounds <{ i8*, i64 }>, <{ i8*, i64 }>* %c, i32 0, i32 0
  %_load.5 = load i8*, i8** %_Pointer.0, align 8
  %_int.6 = getelementptr inbounds i8, i8* %_load.5, i64 9
  %_Pointer.1 = getelementptr inbounds <{ i8*, i64 }>, <{ i8*, i64 }>* %c, i32 0, i32 0
  %_load.6 = load i8*, i8** %_Pointer.1, align 8
  %_int.7 = getelementptr inbounds i8, i8* %_load.6, i64 2
  %0 = ptrtoint i8* %_int.6 to i64
  %1 = ptrtoint i8* %_int.7 to i64
  %2 = sub i64 %0, %1
  %_ptr.0 = sdiv exact i64 %2, ptrtoint (i8* getelementptr (i8, i8* null, i32 1) to i64)
  call void @__PRINTF(i64 %_ptr.0)
  call void @__PRINTF.1(i8 32)
  %_type.0 = call <{ i8*, i64 }> @TO_UPPER(<{ i8*, i64 }> <{ i8* getelementptr inbounds ([10 x i8], [10 x i8]* @str.2, i32 0, i32 0), i64 9 }>)
  call void @__PRINTF.4(<{ i8*, i64 }> %_type.0)
  call void @__PRINTF.1(i8 32)
  %_type.1 = call <{ i8*, i64 }> @MID(<{ i8*, i64 }> <{ i8* getelementptr inbounds ([12 x i8], [12 x i8]* @str.3, i32 0, i32 0), i64 11 }>, i64 2, i64 3)
  call void @__PRINTF.4(<{ i8*, i64 }> %_type.1)
  call void @__PRINTF.1(i8 10)
  store double 2.300000e+00, double* %r1, align 8
  %_load.7 = load double, double* %r1, align 8
  %_load.8 = load i64, i64* %i1, align 8
  %_real.1 = sitofp i64 %_load.8 to double
  %_real.2 = fadd double %_load.7, %_real.1
  store double %_real.2, double* %r2, align 8
  store i1 true, i1* %b1, align 1
  store i1 false, i1* %b2, align 1
  %_load.9 = load i1, i1* %b1, align 1
  call void @__PRINTF.3(i1 %_load.9)
  call void @__PRINTF.1(i8 32)
  %_int.8 = call i64 @.llvmtest.recFib(i64 10)
  call void @__PRINTF(i64 %_int.8)
  call void @__PRINTF.1(i8 10)
  br i1 false, label %then, label %else

then:                                             ; preds = %entry
  call void @__PRINTF.1(i8 99)
  call void @__PRINTF.1(i8 10)
  br label %continue

else:                                             ; preds = %entry
  br i1 false, label %then1, label %else2

continue:                                         ; preds = %continue3, %then
  %t11 = alloca <{ i64, i64, i1 }>, align 8
  %_t1t.0 = getelementptr inbounds <{ i64, i64, i1 }>, <{ i64, i64, i1 }>* %t11, i32 0, i32 0
  store i64 3, i64* %_t1t.0, align 8
  %_t1t.1 = getelementptr inbounds <{ i64, i64, i1 }>, <{ i64, i64, i1 }>* %t11, i32 0, i32 0
  %_load.10 = load i64, i64* %_t1t.1, align 8
  call void @__PRINTF(i64 %_load.10)
  call void @__PRINTF.1(i8 10)
  %t12 = alloca <{ i8*, i64, <{ i64, i64, double }> }>, align 8
  %t13 = alloca <{ i8*, i64, <{ i64, i64, double }> }>, align 8
  %t21 = alloca <{ <{ i8**, i64, i64 }>, <{ i64, i64, i8** }> }>, align 8
  %_load.11 = load i1, i1* %b1, align 1
  %_load.12 = load i1, i1* %b2, align 1
  %_bool.4 = or i1 %_load.11, %_load.12
  %_bool.5 = xor i1 %_bool.4, true
  %_load.13 = load i1, i1* %b2, align 1
  %_bool.6 = and i1 %_load.13, %_bool.5
  %_load.14 = load i64, i64* %i2, align 8
  %_int.9 = sub i64 0, %_load.14
  %_load.15 = load i64, i64* %i1, align 8
  %_bool.7 = icmp eq i64 %_load.15, %_int.9
  %_bool.8 = or i1 %_bool.6, %_bool.7
  store i1 %_bool.8, i1* %b1, align 1
  %_load.16 = load double, double* %r2, align 8
  %_load.17 = load double, double* %r1, align 8
  %_real.3 = fadd double %_load.16, %_load.17
  store double %_real.3, double* %r1, align 8
  %_load.18 = load i64, i64* %i2, align 8
  %_real.4 = sitofp i64 %_load.18 to double
  %_load.19 = load double, double* %r1, align 8
  %_real.5 = fadd double %_real.4, %_load.19
  store double %_real.5, double* %r2, align 8
  %_load.20 = load double, double* %r1, align 8
  call void @__PRINTF.2(double %_load.20)
  call void @__PRINTF.1(i8 32)
  %_load.21 = load double, double* %r2, align 8
  call void @__PRINTF.2(double %_load.21)
  call void @__PRINTF.1(i8 10)
  call void @.llvmtest.swap.12(double* %r1, double* %r2)
  %_load.25 = load double, double* %r1, align 8
  call void @__PRINTF.2(double %_load.25)
  call void @__PRINTF.1(i8 32)
  %_load.26 = load double, double* %r2, align 8
  call void @__PRINTF.2(double %_load.26)
  call void @__PRINTF.1(i8 10)
  %_load.27 = load double, double* %r2, align 8
  %_real.7 = fdiv double %_load.27, 4.000000e+00
  %_int.10 = fptosi double %_real.7 to i64
  store i64 %_int.10, i64* %i2, align 8
  store <{ i8*, i64 }> <{ i8* getelementptr inbounds ([4 x i8], [4 x i8]* @str.4, i32 0, i32 0), i64 3 }>, <{ i8*, i64 }>* %c, align 1
  ret void

then1:                                            ; preds = %else
  call void @__PRINTF.1(i8 101)
  call void @__PRINTF.1(i8 10)
  br label %continue3

else2:                                            ; preds = %else
  call void @__PRINTF.1(i8 102)
  call void @__PRINTF.1(i8 10)
  br label %continue3

continue3:                                        ; preds = %else2, %then1
  call void @__PRINTF.3(i1 true)
  call void @__PRINTF.1(i8 10)
  br label %continue
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

define weak i64 @.llvmtest.recFib(i64 %n) {
entry:
  %_bool.0 = icmp eq i64 %n, 1
  %_bool.1 = icmp eq i64 %n, 2
  %_bool.2 = or i1 %_bool.0, %_bool.1
  br i1 %_bool.2, label %then, label %continue

then:                                             ; preds = %entry
  ret i64 1
  br label %continue

continue:                                         ; preds = %then, %entry
  %_int.0 = sub i64 %n, 1
  %_int.1 = call i64 @.llvmtest.recFib(i64 %_int.0)
  %_int.2 = sub i64 %n, 2
  %_int.3 = call i64 @.llvmtest.recFib(i64 %_int.2)
  %_int.4 = add i64 %_int.1, %_int.3
  ret i64 %_int.4
}

define weak void @.llvmtest.swap(i64* %a, i64* %b) {
entry:
  %tmp = alloca i64, align 8
  %_load.0 = load i64, i64* %a, align 8
  store i64 %_load.0, i64* %tmp, align 8
  %_load.1 = load i64, i64* %b, align 8
  store i64 %_load.1, i64* %a, align 8
  %_load.2 = load i64, i64* %tmp, align 8
  store i64 %_load.2, i64* %b, align 8
  ret void
}

define weak void @.llvmtest.swap.12(double* %a, double* %b) {
entry:
  %tmp = alloca double, align 8
  %_load.22 = load double, double* %a, align 8
  store double %_load.22, double* %tmp, align 8
  %_load.23 = load double, double* %b, align 8
  store double %_load.23, double* %a, align 8
  %_load.24 = load double, double* %tmp, align 8
  store double %_load.24, double* %b, align 8
  ret void
}

attributes #0 = { "frame-pointer"="none" }

!llvm.dbg.cu = !{!0}

!0 = distinct !DICompileUnit(language: DW_LANG_C, file: !1, producer: "PseudoCode", isOptimized: false, runtimeVersion: 0, emissionKind: FullDebug, splitDebugInlining: false)
!1 = !DIFile(filename: "llvmtest.p", directory: "")

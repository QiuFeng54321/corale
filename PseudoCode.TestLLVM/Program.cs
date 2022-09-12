using System.Runtime.InteropServices;
using LLVMSharp.Interop;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal delegate void Print(string d);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate string? Read();

internal class Program
{
    private static void Print(string s)
    {
        Console.WriteLine(s);
    }

    private static string? Read()
    {
        return Console.ReadLine();
    }

    private static unsafe sbyte* ToSByte(string str)
    {
        return (sbyte*)Marshal.StringToHGlobalAuto(str);
    }

    public static void Main()
    {
        LLVM.LinkInMCJIT();
        LLVM.InitializeX86TargetMC();
        LLVM.InitializeX86Target();
        LLVM.InitializeX86TargetInfo();
        LLVM.InitializeX86AsmParser();
        LLVM.InitializeX86AsmPrinter();

        var module = LLVMModuleRef.CreateWithName("Module");
        var builder = module.Context.CreateBuilder();
        var engine = module.CreateMCJITCompiler();
        var printFunctionType =
            LLVMTypeRef.CreateFunction(LLVMTypeRef.Void, new[] { LLVMTypeRef.CreatePointer(LLVMTypeRef.Int8, 0) });
        var printFunction = module.AddFunction("print", printFunctionType);
        printFunction.Linkage = LLVMLinkage.LLVMExternalLinkage;
        Delegate printDelegate = new Print(Print);
        var pointerToCSharpPrintFunction = Marshal.GetFunctionPointerForDelegate(printDelegate);
        engine.AddGlobalMapping(printFunction, pointerToCSharpPrintFunction);


        // adding function "read" to module
        var readFunctionType =
            LLVMTypeRef.CreateFunction(LLVMTypeRef.CreatePointer(LLVMTypeRef.Int8, 0), new[] { LLVMTypeRef.Void });
        var readFunction = module.AddFunction("read", readFunctionType);
        readFunction.Linkage = LLVMLinkage.LLVMExternalLinkage;
        Delegate readDelegate = new Read(Read);
        var pointerToCSharpReadFunction = Marshal.GetFunctionPointerForDelegate(readDelegate);
        engine.AddGlobalMapping(readFunction, pointerToCSharpReadFunction);

        // adding function "main" to module
        var customFunctionType = LLVMTypeRef.CreateFunction(LLVMTypeRef.Int32, Array.Empty<LLVMTypeRef>());
        var customFunction = module.AddFunction("test", customFunctionType);
        var cbb = customFunction.AppendBasicBlock("entry2");
        builder.PositionAtEnd(cbb);
        builder.BuildRet(
            LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, 3)
        );


        // adding function "main" to module
        var mainFunctionType = LLVMTypeRef.CreateFunction(LLVMTypeRef.Void, Array.Empty<LLVMTypeRef>());
        var mainFunction = module.AddFunction("main", mainFunctionType);
        mainFunction.Linkage = LLVMLinkage.LLVMExternalLinkage;
        var bb = mainFunction.AppendBasicBlock("entry");
        builder.PositionAtEnd(bb);
        builder.BuildCall(printFunction, new[] { builder.BuildGlobalStringPtr("Please enter a value:") });
        var enteredValue = builder.BuildCall(readFunction, Array.Empty<LLVMValueRef>());
        builder.BuildCall(printFunction, new[] { builder.BuildGlobalStringPtr("You have entered:") });
        builder.BuildCall(printFunction, new[] { enteredValue });
        builder.BuildRetVoid();

        var main = module.GetNamedFunction("main");

        module.Dump();
        engine.RunFunction(main, Array.Empty<LLVMGenericValueRef>());
    }
}
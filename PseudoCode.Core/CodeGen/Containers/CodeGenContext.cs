using LLVMSharp.Interop;
using PseudoCode.Core.Analyzing;
using PseudoCode.Core.CodeGen.Operator;
using PseudoCode.Core.Parsing;

namespace PseudoCode.Core.CodeGen.Containers;

/// <summary>
/// </summary>
public class CodeGenContext
{
    public readonly Analysis Analysis;
    public readonly Stack<Expression> ExpressionStack = new();

    public readonly Namespace GlobalNamespace = new("global", null);
    public readonly NameGenerator NameGenerator;
    public readonly OperatorResolverMap OperatorResolverMap;
    public LLVMExecutionEngineRef Engine;
    public CompilationUnit MainCompilationUnit;
    public PseudoCodeCompiler PseudoCodeCompiler;

    public CodeGenContext()
    {
        LLVM.LinkInMCJIT();
        LLVM.InitializeX86TargetMC();
        LLVM.InitializeX86Target();
        LLVM.InitializeX86TargetInfo();
        LLVM.InitializeX86AsmParser();
        LLVM.InitializeX86AsmPrinter();
        Analysis = new Analysis();
        NameGenerator = new NameGenerator();
        OperatorResolverMap = new OperatorResolverMap();
        OperatorResolverMap.Initialize();
        GlobalNamespace.AddNamespace(GlobalNamespace);
        // CompilationUnit.MainFunction.GeneratePrototype(this);
    }

    public void MakeMainCompilationUnit(string path)
    {
        MainCompilationUnit = new CompilationUnit(this, path)
        {
            Namespace = GlobalNamespace
        };
        Engine = MainCompilationUnit.Module.CreateMCJITCompiler();
    }
}
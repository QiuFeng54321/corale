using LLVMSharp.Interop;
using PseudoCode.Core.Analyzing;
using PseudoCode.Core.CodeGen.Operator;

namespace PseudoCode.Core.CodeGen.Containers;

/// <summary>
/// </summary>
public class CodeGenContext
{
    public readonly Analysis Analysis;
    public readonly CompilationUnit CompilationUnit;
    public readonly Stack<Expression> ExpressionStack = new();

    public readonly Namespace GlobalNamespace = new("global", null);
    public readonly NameGenerator NameGenerator;
    public readonly OperatorResolverMap OperatorResolverMap;
    public readonly Stack<Symbol> TypeLookupStack = new();
    public LLVMBuilderRef Builder;
    public LLVMExecutionEngineRef Engine;
    public LLVMModuleRef Module;

    public CodeGenContext(string moduleName = "Module")
    {
        LLVM.LinkInMCJIT();
        LLVM.InitializeX86TargetMC();
        LLVM.InitializeX86Target();
        LLVM.InitializeX86TargetInfo();
        LLVM.InitializeX86AsmParser();
        LLVM.InitializeX86AsmPrinter();
        Analysis = new Analysis();
        Module = LLVMModuleRef.CreateWithName(moduleName);
        Builder = Module.Context.CreateBuilder();
        NameGenerator = new NameGenerator();
        Engine = Module.CreateMCJITCompiler();
        OperatorResolverMap = new OperatorResolverMap();
        OperatorResolverMap.Initialize();
        CompilationUnit = new CompilationUnit
        {
            Namespace = GlobalNamespace
        };
        CompilationUnit.MakeMainFunction(this, moduleName);
        // CompilationUnit.MainFunction.GeneratePrototype(this);
    }
}
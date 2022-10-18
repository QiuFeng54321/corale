using LLVMSharp.Interop;
using PseudoCode.Core.Analyzing;
using PseudoCode.Core.CodeGen.Operator;

namespace PseudoCode.Core.CodeGen.Containers;

/// <summary>
/// </summary>
public class CodeGenContext
{
    public readonly Stack<Expression> ExpressionStack = new();
    public readonly Stack<Symbol> TypeLookupStack = new();
    public Analysis Analysis;
    public LLVMBuilderRef Builder;
    public CompilationUnit CompilationUnit;
    public LLVMExecutionEngineRef Engine;

    public Namespace GlobalNamespace = new("global", null);
    public LLVMModuleRef Module;
    public NameGenerator NameGenerator;
    public OperatorResolverMap OperatorResolverMap;

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
        CompilationUnit.MakeMainFunction(this);
        // CompilationUnit.MainFunction.GeneratePrototype(this);
    }
}
using LLVMSharp.Interop;
using PseudoCode.Core.Analyzing;

namespace PseudoCode.Core.CodeGen;

/// <summary>
/// </summary>
public class CodeGenContext
{
    public readonly Stack<Expression> ExpressionStack = new();
    public readonly Stack<Symbol> TypeLookupStack = new();
    public Analysis Analysis;
    public LLVMBuilderRef Builder;
    public LLVMExecutionEngineRef Engine;
    public LLVMModuleRef Module;
    public NameGenerator NameGenerator;
    public ProgramRoot Root;

    public CodeGenContext(string moduleName = "Module")
    {
        LLVM.LinkInMCJIT();
        LLVM.InitializeX86TargetMC();
        LLVM.InitializeX86Target();
        LLVM.InitializeX86TargetInfo();
        LLVM.InitializeX86AsmParser();
        LLVM.InitializeX86AsmPrinter();
        Root = new ProgramRoot
        {
            Namespace = new Namespace("global", null)
        };
        Analysis = new Analysis();
        Module = LLVMModuleRef.CreateWithName(moduleName);
        Builder = Module.Context.CreateBuilder();
        NameGenerator = new NameGenerator();
        Engine = Module.CreateMCJITCompiler();
    }
}
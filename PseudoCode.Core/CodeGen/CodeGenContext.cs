using LLVMSharp.Interop;
using PseudoCode.Core.Analyzing;
using PseudoCode.Core.CodeGen;

namespace PseudoCode.Core.Parsing;

/// <summary>
/// </summary>
public class CodeGenContext
{
    public Analysis Analysis;
    public LLVMBuilderRef IRBuilder;
    public LLVMModuleRef Module;
    public NameGenerator NameGenerator;
    public ProgramRoot Root;

    public CodeGenContext(string moduleName = "Module")
    {
        Root = new ProgramRoot();
        Analysis = new Analysis();
        Module = LLVMModuleRef.CreateWithName(moduleName);
        IRBuilder = Module.Context.CreateBuilder();
        NameGenerator = new NameGenerator();
    }
}
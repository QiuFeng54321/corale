using LLVMSharp.Interop;
using PseudoCode.Core.Analyzing;

namespace PseudoCode.Core.CodeGen;

/// <summary>
/// </summary>
public class CodeGenContext
{
    public Analysis Analysis;
    public LLVMBuilderRef Builder;
    public LLVMModuleRef Module;
    public NameGenerator NameGenerator;
    public ProgramRoot Root;

    public CodeGenContext(string moduleName = "Module")
    {
        Root = new ProgramRoot
        {
            Namespace = new Namespace("global", null)
        };
        Analysis = new Analysis();
        Module = LLVMModuleRef.CreateWithName(moduleName);
        Builder = Module.Context.CreateBuilder();
        NameGenerator = new NameGenerator();
    }
}
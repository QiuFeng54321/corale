using PseudoCode.Core.Analyzing;
using Type = PseudoCode.Core.Runtime.Types.Type;

namespace PseudoCode.Core.Runtime.Operations;

public class OpenFileOperation : FileOperation
{
    public FileMode FileMode;
    public FileAccess FileAccess;
    public bool IsRandom;

    public OpenFileOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()                                                  
    {
        base.Operate();
        var pathInstance = ParentScope.FindTypeDefinition(Type.StringId).Type.CastFrom(Program.RuntimeStack.Pop());
        var path = pathInstance.Get<string>();
        var stream = new PseudoFileStream(path, FileMode, FileAccess, IsRandom);
        stream.Open();
        Program.OpenFiles.Add(path, stream);
    }
    public override void MetaOperate()                                                  
    {
        base.Operate();
        PopAndCheckPath();
    }
}
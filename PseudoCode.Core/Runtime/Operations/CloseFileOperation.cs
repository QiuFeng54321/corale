using Type = PseudoCode.Core.Runtime.Types.Type;

namespace PseudoCode.Core.Runtime.Operations;

public class CloseFileOperation : FileOperation
{
    public CloseFileOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }
    
    public override void Operate()
    {
        base.Operate();
        var stringType = ParentScope.FindTypeDefinition(Type.StringId).Type;
        var pathInstance = stringType.CastFrom(Program.RuntimeStack.Pop());
        var path = pathInstance.Get<string>();
        Program.OpenFiles[path].Close();
    }

    public override void MetaOperate()
    {
        base.Operate();
        PopAndCheckPath();
    }
}
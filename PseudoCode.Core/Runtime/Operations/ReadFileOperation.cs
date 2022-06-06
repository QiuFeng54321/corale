using Type = PseudoCode.Core.Runtime.Types.Type;

namespace PseudoCode.Core.Runtime.Operations;

public class ReadFileOperation : FileOperation
{
    public ReadFileOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        var instance = Program.RuntimeStack.Pop();
        var stringType = ParentScope.FindTypeDefinition(Type.StringId).Type;
        var path = PopPathAtRuntime();
        var str = Program.OpenFiles[path].ReadLine();
        var strInstance = stringType.Instance(str);
        instance.Type.Assign(instance, strInstance);
    }

    public override void MetaOperate()
    {
        base.Operate();
        var instance = Program.TypeCheckStack.Pop();
        PopAndCheckPath();
    }
}
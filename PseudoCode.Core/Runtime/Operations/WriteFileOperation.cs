using Type = PseudoCode.Core.Runtime.Types.Type;

namespace PseudoCode.Core.Runtime.Operations;

public class WriteFileOperation : FileOperation
{
    public WriteFileOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        var instance = Program.RuntimeStack.Pop();
        var stringType = Program.FindTypeDefinition(Type.StringId).Type;
        var path = PopPathAtRuntime();
        var content = stringType.CastFrom(instance).Get<string>();
        Program.OpenFiles[path].Write(content);
    }

    public override void MetaOperate()
    {
        base.Operate();
        var instance = Program.TypeCheckStack.Pop();
        PopAndCheckPath();
    }
    public override string ToPlainString() => "Write file";
}
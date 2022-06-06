using Type = PseudoCode.Core.Runtime.Types.Type;

namespace PseudoCode.Core.Runtime.Operations;

public class GetRecordOperation : FileOperation
{
    public GetRecordOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }
    public override void Operate()
    {
        base.Operate();
        var instance = Program.RuntimeStack.Pop();
        var path = PopPathAtRuntime();
        var got = Program.OpenFiles[path].Get();
        instance.Type.Assign(instance, got);
    }


    public override void MetaOperate()
    {
        base.Operate();
        var instance = Program.TypeCheckStack.Pop();
        PopAndCheckPath();
    }
}
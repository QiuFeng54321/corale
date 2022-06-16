using Type = PseudoCode.Core.Runtime.Types.Type;

namespace PseudoCode.Core.Runtime.Operations;

public class SeekOperation : FileOperation
{
    public SeekOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        var addressInstance = Program.RuntimeStack.Pop();
        var address = Program.FindDefinition(Type.IntegerId).Type.CastFrom(addressInstance).Get<int>();
        var path = PopPathAtRuntime();
        Program.OpenFiles[path].Seek(address);
    }

    public override void MetaOperate()
    {
        base.Operate();
        var instance = Program.TypeCheckStack.Pop();
        PopAndCheckPath();
    }
    public override string ToPlainString() => "Seek";
}
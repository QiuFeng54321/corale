using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.Runtime.Operations;

public class MemberAccessOperation : Operation
{
    public string MemberName;
    public MemberAccessOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        var accessed = Program.RuntimeStack.Pop();
        Program.RuntimeStack.Push(accessed.Type.MemberAccess(accessed, MemberName));
    }

    public override void MetaOperate()
    {
        base.MetaOperate();
        var accessed = Program.TypeCheckStack.Pop();
        Program.TypeCheckStack.Push(new TypeInfo {
            Type = accessed.Type.MemberAccessResultType(MemberName),
            IsReference = true,
            SourceRange = SourceRange
        });
    }

    public override string ToPlainString() => $"Access {MemberName}";
}
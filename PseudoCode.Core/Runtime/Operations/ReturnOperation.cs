using PseudoCode.Core.Runtime.Errors;

namespace PseudoCode.Core.Runtime.Operations;

public class ReturnOperation : Operation
{
    public ReturnOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        throw new ReturnBreak($"Return statement not reaching a function", this);
    }

    public override void MetaOperate()
    {
        base.MetaOperate();
        Program.TypeCheckStack.Pop();
    }

    public override string ToPlainString() => "Return";
}
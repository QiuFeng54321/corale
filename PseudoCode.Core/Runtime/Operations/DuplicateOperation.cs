namespace PseudoCode.Core.Runtime.Operations;

public class DuplicateOperation : Operation
{
    public DuplicateOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        Program.RuntimeStack.Push(Program.RuntimeStack.Peek());
    }

    public override void MetaOperate()
    {
        base.MetaOperate();
        Program.TypeCheckStack.Push(Program.TypeCheckStack.Peek());
    }

    public override string ToPlainString()
    {
        return "Duplicate";
    }
}
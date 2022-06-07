namespace PseudoCode.Core.Runtime.Operations;

public class SwapOperation : Operation
{
    public SwapOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        var first = Program.RuntimeStack.Pop();
        var second = Program.RuntimeStack.Pop();
        Program.RuntimeStack.Push(first);
        Program.RuntimeStack.Push(second);
    }
    public override void MetaOperate()
    {
        base.MetaOperate();
        var first = Program.TypeCheckStack.Pop();
        var second = Program.TypeCheckStack.Pop();
        Program.TypeCheckStack.Push(first);
        Program.TypeCheckStack.Push(second);
    }

    public override string ToPlainString() => "Swap";
}
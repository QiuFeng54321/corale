namespace PseudoCode.Core.Runtime.Operations;

public class UnaryOperation : Operation
{
    public int OperatorMethod;

    public UnaryOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        var to = Program.RuntimeStack.Pop();
        to = to.Type.UnaryOperators[OperatorMethod](to);
        Program.RuntimeStack.Push(to);
    }

    public override string ToPlainString()
    {
        return string.Format(strings.UnaryOperation_ToPlainString, OperatorMethod);
    }
}
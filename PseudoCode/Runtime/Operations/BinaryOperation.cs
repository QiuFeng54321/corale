namespace PseudoCode.Runtime.Operations;

public class BinaryOperation : Operation
{
    public int OperatorMethod;

    public BinaryOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        var value = Program.RuntimeStack.Pop();
        var to = Program.RuntimeStack.Pop();
        to = to.Type.BinaryOperators[OperatorMethod](to, value);
        Program.RuntimeStack.Push(to);
    }

    public override string ToPlainString()
    {
        return $"Binary {OperatorMethod}";
    }
}
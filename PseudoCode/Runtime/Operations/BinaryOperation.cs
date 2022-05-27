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
        var value = ParentScope.RuntimeStack.Pop();
        var to = ParentScope.RuntimeStack.Pop();
        to = to.Type.BinaryOperators[OperatorMethod](to, value);
        ParentScope.RuntimeStack.Push(to);
    }

    public override string ToPlainString()
    {
        return $"Binary {OperatorMethod}";
    }
}
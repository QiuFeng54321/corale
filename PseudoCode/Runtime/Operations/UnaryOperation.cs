namespace PseudoCode.Runtime.Operations;

public class UnaryOperation : Operation
{
    public int OperatorMethod;
    public override void Operate()
    {
        base.Operate();
        var to = ParentScope.RuntimeStack.Pop();
        to = to.Type.UnaryOperators[OperatorMethod](to);
        ParentScope.RuntimeStack.Push(to);
    }

    public override string ToString()
    {
        return $"Unary {OperatorMethod}";
    }

    public UnaryOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }
}
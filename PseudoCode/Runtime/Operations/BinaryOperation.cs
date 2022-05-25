namespace PseudoCode.Runtime.Operations;

public class BinaryOperation : Operation
{
    public int OperatorMethod;
    public override void Operate()
    {
        base.Operate();
        var value = ParentScope.RuntimeStack.Pop();
        var to = ParentScope.RuntimeStack.Pop();
        to = to.Type.BinaryOperators[OperatorMethod](to, value);
        ParentScope.RuntimeStack.Push(to);
    }

    public override string ToString()
    {
        return $"Binary {OperatorMethod}";
    }

    public BinaryOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }
}
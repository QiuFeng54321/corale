namespace PseudoCode.Runtime.Operations;

public class BinaryOperation : Operation
{
    public int OperatorMethod;
    public override void Operate()
    {
        base.Operate();
        var value = Scope.RuntimeStack.Pop();
        var to = Scope.RuntimeStack.Pop();
        to = to.Type.BinaryOperators[OperatorMethod](to, value);
        Scope.RuntimeStack.Push(to);
    }

    public override string ToString()
    {
        return $"Binary {OperatorMethod}";
    }
}
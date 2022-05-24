namespace PseudoCode.Runtime.Operations;

public class UnaryOperation : Operation
{
    public int OperatorMethod;
    public override void Operate()
    {
        base.Operate();
        var to = Scope.RuntimeStack.Pop();
        to = to.Type.UnaryOperators[OperatorMethod](to);
        Scope.RuntimeStack.Push(to);
    }

    public override string ToString()
    {
        return $"Unary {OperatorMethod}";
    }
}
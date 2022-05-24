namespace PseudoCode.Runtime.Operations;

public class AddOperation : Operation
{
    public override void Operate()
    {
        base.Operate();
        var value = Scope.RuntimeStack.Pop();
        var to = Scope.RuntimeStack.Pop();
        to = to.Type.Add(to, value);
        Scope.RuntimeStack.Push(to);
    }

    public override string ToString()
    {
        return $"Add";
    }
}
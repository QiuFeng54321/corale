namespace PseudoCode.Runtime.Operations;

public class AssignmentOperation : Operation
{
    public override void Operate()
    {
        var value = Scope.RuntimeStack.Pop();
        var to = Scope.RuntimeStack.Pop();
        to.Type.Assign(to, value);
        Scope.RuntimeStack.Push(to);
    }

    public override string ToString()
    {
        return $"Assign";
    }
}
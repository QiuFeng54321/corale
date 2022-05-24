namespace PseudoCode.Runtime.Operations;

public class LoadOperation : Operation
{
    public string LoadName;

    public override void Operate()
    {
        base.Operate();
        Scope.RuntimeStack.Push(new ReferenceInstance { Scope = Scope, ReferenceName = LoadName });
    }

    public override string ToString()
    {
        return $"Push ref {LoadName}";
    }
}
namespace PseudoCode.Runtime.Operations;

public class LoadImmediateOperation : Operation
{
    public Instance Intermediate;

    public override void Operate()
    {
        base.Operate();
        Scope.RuntimeStack.Push(Intermediate);
    }

    public override string ToString()
    {
        return $"Push immediate {Intermediate}";
    }
}
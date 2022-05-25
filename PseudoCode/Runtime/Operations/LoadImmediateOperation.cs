namespace PseudoCode.Runtime.Operations;

public class LoadImmediateOperation : Operation
{
    public Instance Intermediate;

    public override void Operate()
    {
        base.Operate();
        ParentScope.RuntimeStack.Push(Intermediate);
    }

    public override string ToString()
    {
        return $"Push immediate {Intermediate}";
    }

    public LoadImmediateOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }
}
namespace PseudoCode.Runtime.Operations;

public class LoadImmediateOperation : Operation
{
    public Instance Intermediate;

    public LoadImmediateOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        ParentScope.RuntimeStack.Push(Intermediate);
    }

    public override string ToPlainString()
    {
        return $"Push immediate {Intermediate}";
    }
}
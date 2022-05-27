namespace PseudoCode.Runtime.Operations;

public class LoadOperation : Operation
{
    public string LoadName;

    public LoadOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        ParentScope.RuntimeStack.Push(new ReferenceInstance(ParentScope, Program)
            { ReferenceAddress = ParentScope.FindInstanceAddress(LoadName) });
    }

    public override string ToPlainString()
    {
        return $"Push ref {LoadName}";
    }
}
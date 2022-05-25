namespace PseudoCode.Runtime.Operations;

public class AssignmentOperation : Operation
{
    public override void Operate()
    {
        var value = ParentScope.RuntimeStack.Pop();
        var to = ParentScope.RuntimeStack.Pop();
        to.Type.Assign(to, value);
    }

    public override string ToPlainString()
    {
        return "Assign";
    }

    public AssignmentOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }
}
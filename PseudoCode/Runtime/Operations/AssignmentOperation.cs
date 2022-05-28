namespace PseudoCode.Runtime.Operations;

public class AssignmentOperation : Operation
{
    public AssignmentOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        var value = Program.RuntimeStack.Pop();
        var to = Program.RuntimeStack.Pop();
        to.Type.Assign(to, value);
    }

    public override string ToPlainString()
    {
        return "Assign";
    }
}
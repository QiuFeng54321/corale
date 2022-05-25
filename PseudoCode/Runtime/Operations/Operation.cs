namespace PseudoCode.Runtime.Operations;

public class Operation
{
    public Scope ParentScope;
    public PseudoProgram Program;

    public Operation(Scope parentScope, PseudoProgram program)
    {
        ParentScope = parentScope;
        Program = program;
    }

    public virtual void Operate()
    {
    }
}
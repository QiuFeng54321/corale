namespace PseudoCode.Runtime.Operations;

public class IfOperation : Operation
{
    public Scope TestExpressionScope;
    public Operation TrueBlock, FalseBlock;

    public override void Operate()
    {
        base.Operate();
        TestExpressionScope.Operate();
        var test = ParentScope.FindType("BOOLEAN").CastFrom(TestExpressionScope.RuntimeStack.Pop());
        if (test.Get<bool>())
        {
            TrueBlock.Operate();
        }
        else
        {
            FalseBlock?.Operate();
        }
    }

    public override string ToString()
    {
        return $"Test:\n{TestExpressionScope}\nTrue:\n{TrueBlock}\n{(FalseBlock != null ? $"False:\n{FalseBlock}" : "")}";
    }

    public IfOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }
}
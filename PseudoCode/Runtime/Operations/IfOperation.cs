namespace PseudoCode.Runtime.Operations;

public class IfOperation : Operation
{
    public Scope TestExpressionScope;
    public Operation TrueBlock, FalseBlock;

    public IfOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        TestExpressionScope.Operate();
        var test = ParentScope.FindType(Type.BooleanId).HandledCastFrom(TestExpressionScope.RuntimeStack.Pop());
        if (test.Get<bool>())
            TrueBlock.Operate();
        else
            FalseBlock?.Operate();
    }

    public override string ToPlainString() => "If";

    public override string ToString(int depth)
    {
        return
            $"{Indent(depth)}Branch:\n{TestExpressionScope.ToString(depth)}\n{Indent(depth)}True:\n{TrueBlock.ToString(depth)}\n{(FalseBlock != null ? $"{Indent(depth)}False:\n{FalseBlock.ToString(depth)}" : "")}";
    }
}
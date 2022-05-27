namespace PseudoCode.Runtime.Operations;

public class RepeatOperation : Operation
{
    public Scope TestExpressionScope;
    public Operation RepeatBlock;
    public bool TestFirst = true;

    public override void Operate()
    {
        base.Operate();
        bool test = true;
        if (TestFirst)
        {
            TestExpressionScope.Operate();
            test = ParentScope.FindType("BOOLEAN").CastFrom(TestExpressionScope.RuntimeStack.Pop()).Get<bool>();
        }

        while (test)
        {
            RepeatBlock.Operate();
            TestExpressionScope.Operate();
            test = ParentScope.FindType("BOOLEAN").CastFrom(TestExpressionScope.RuntimeStack.Pop()).Get<bool>();
        }
    }

    public override string ToString(int depth)
    {
        return
            TestFirst
                ? $"{Indent(depth)}While:\n{TestExpressionScope.ToString(depth)}\n{Indent(depth)}Do:\n{RepeatBlock.ToString(depth)}"
                : $"{Indent(depth)}Do:\n{RepeatBlock.ToString(depth)}\n{Indent(depth)}While:\n{TestExpressionScope.ToString(depth)}";
    }

    public RepeatOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }
}
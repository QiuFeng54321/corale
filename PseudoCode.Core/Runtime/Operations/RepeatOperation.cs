namespace PseudoCode.Core.Runtime.Operations;

public class RepeatOperation : Operation
{
    public Operation RepeatBlock;
    public Scope TestExpressionScope;
    public bool TestFirst = true;

    public RepeatOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        var test = true;
        if (TestFirst)
        {
            TestExpressionScope.HandledOperate();
            test = ParentScope.FindTypeDefinition(Type.BooleanId).Type.HandledCastFrom(Program.RuntimeStack.Pop()).Get<bool>();
        }

        while (test)
        {
            RepeatBlock.HandledOperate();
            TestExpressionScope.HandledOperate();
            test = ParentScope.FindTypeDefinition(Type.BooleanId).Type.HandledCastFrom(Program.RuntimeStack.Pop()).Get<bool>();
        }
    }

    public override void MetaOperate()
    {
        base.MetaOperate();
        TestExpressionScope.MetaOperate();
        RepeatBlock.MetaOperate();
    }

    public override string ToPlainString() => TestFirst ? "While" : "Repeat-Until";

    public override string ToString(int depth)
    {
        return
            TestFirst
                ? string.Format(strings.RepeatOperation_ToString_While, Indent(depth), TestExpressionScope.ToString(depth), Indent(depth), RepeatBlock.ToString(depth))
                : string.Format(strings.RepeatOperation_ToString_Repeat, Indent(depth), RepeatBlock.ToString(depth), Indent(depth), TestExpressionScope.ToString(depth));
    }
}
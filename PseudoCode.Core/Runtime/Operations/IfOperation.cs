namespace PseudoCode.Core.Runtime.Operations;

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
        TestExpressionScope.HandledOperate();
        var test = ParentScope.FindTypeDefinition(Type.BooleanId).Type.HandledCastFrom(Program.RuntimeStack.Pop());
        if (test.Get<bool>())
            TrueBlock.HandledOperate();
        else
            FalseBlock?.HandledOperate();
    }

    public override void MetaOperate()
    {
        base.MetaOperate();
        TestExpressionScope.MetaOperate();
        Program.TypeCheckStack.Pop();
        TrueBlock.MetaOperate();
        FalseBlock?.MetaOperate();
    }

    public override string ToPlainString() => "If";

    public override string ToString(int depth)
    {
        return
            string.Format(strings.IfOperation_ToString, Indent(depth), TestExpressionScope.ToString(depth), Indent(depth), TrueBlock.ToString(depth), (FalseBlock != null ? string.Format(strings.IfOperation_ToString_Else, Indent(depth), FalseBlock.ToString(depth)) : ""));
    }
}
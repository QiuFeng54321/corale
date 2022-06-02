using PseudoCode.Core.Analyzing;

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
        var testType = Program.TypeCheckStack.Pop();
        if (!ParentScope.FindTypeDefinition(Type.BooleanId).Type.IsConvertableFrom(testType))
        {
            Program.AnalyserFeedbacks.Add(new Feedback
            {
                Message = $"Test expression is of type {testType} and cannot be converted into BOOLEAN",
                Severity = Feedback.SeverityType.Error,
                SourceRange = TestExpressionScope.SourceRange
            });
        }
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
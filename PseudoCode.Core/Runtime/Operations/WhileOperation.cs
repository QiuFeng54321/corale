using PseudoCode.Core.Analyzing;
using Type = PseudoCode.Core.Runtime.Types.Type;

namespace PseudoCode.Core.Runtime.Operations;

public class WhileOperation : Operation
{
    public Operation RepeatBlock;
    public Scope TestExpressionScope;

    public WhileOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        TestExpressionScope.HandledOperate();
        var test = Program.FindTypeDefinition(Type.BooleanId).Type.HandledCastFrom(Program.RuntimeStack.Pop())
            .Get<bool>();

        while (test)
        {
            RepeatBlock.HandledOperate();
            TestExpressionScope.HandledOperate();
            test = Program.FindTypeDefinition(Type.BooleanId).Type.HandledCastFrom(Program.RuntimeStack.Pop())
                .Get<bool>();
        }
    }

    public override void MetaOperate()
    {
        base.MetaOperate();
        MetaOperateTest();
        RepeatBlock.MetaOperate();
    }

    private void MetaOperateTest()
    {
        TestExpressionScope.MetaOperate();
        var testType = Program.TypeCheckStack.Pop();
        if (!Program.FindTypeDefinition(Type.BooleanId).Type.IsConvertableFrom(testType.Type))
            Program.AnalyserFeedbacks.Add(new Feedback
            {
                Message = $"Test expression is of type {testType} and cannot be converted into BOOLEAN",
                Severity = Feedback.SeverityType.Error,
                SourceRange = TestExpressionScope.SourceRange
            });
    }

    public override string ToPlainString()
    {
        return "While";
    }

    public override string ToString(int depth)
    {
        return string.Format(strings.RepeatOperation_ToString_While, Indent(depth),
            TestExpressionScope.ToString(depth), Indent(depth), RepeatBlock.ToString(depth));
    }
}
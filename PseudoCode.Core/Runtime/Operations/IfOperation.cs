using PseudoCode.Core.Analyzing;
using Type = PseudoCode.Core.Runtime.Types.Type;

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
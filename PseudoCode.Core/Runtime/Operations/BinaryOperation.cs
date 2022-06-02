using PseudoCode.Core.Analyzing;

namespace PseudoCode.Core.Runtime.Operations;

public class BinaryOperation : Operation
{
    public int OperatorMethod;

    public BinaryOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        var value = Program.RuntimeStack.Pop();
        var to = Program.RuntimeStack.Pop();
        to = to.Type.BinaryOperators[OperatorMethod](to, value);
        Program.RuntimeStack.Push(to);
    }

    public override void MetaOperate()
    {
        base.MetaOperate();
        var right = Program.TypeCheckStack.Pop();
        var left = Program.TypeCheckStack.Pop();
        var resType = left.BinaryResultType(OperatorMethod, right);
        if (resType.Id == Type.NullId)
        {
            Program.AnalyserFeedbacks.Add(new Feedback
            {
                Message = $"This operation on {left} and {right} is either not supported or will be casted to another type at runtime",
                Severity = Feedback.SeverityType.Warning,
                SourceRange = SourceRange
            });
        }
        Program.TypeCheckStack.Push(resType);
    }

    public override string ToPlainString()
    {
        return string.Format(strings.BinaryOperation_ToPlainString, OperatorMethod);
    }
}
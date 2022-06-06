using PseudoCode.Core.Analyzing;
using Type = PseudoCode.Core.Runtime.Types.Type;

namespace PseudoCode.Core.Runtime.Operations;

public class UnaryOperation : Operation
{
    public int OperatorMethod;

    public UnaryOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        var to = Program.RuntimeStack.Pop();
        to = to.Type.UnaryOperators[OperatorMethod](to);
        Program.RuntimeStack.Push(to);
    }

    public override void MetaOperate()
    {
        base.MetaOperate();
        var to = Program.TypeCheckStack.Pop();
        var resType = to.UnaryResultType(OperatorMethod);
        if (resType.Id == Type.NullId)
        {
            Program.AnalyserFeedbacks.Add(new Feedback
            {
                Message = $"This operation on {to} is either not supported or will be casted to another type at runtime",
                Severity = Feedback.SeverityType.Warning,
                SourceRange = SourceRange
            });
        }
        Program.TypeCheckStack.Push(resType);
    }

    public override string ToPlainString()
    {
        return string.Format(strings.UnaryOperation_ToPlainString, OperatorMethod);
    }
}
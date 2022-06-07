using PseudoCode.Core.Analyzing;
using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Types;
using Type = PseudoCode.Core.Runtime.Types.Type;

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
        var resType = left.Type.BinaryResultType(OperatorMethod, right.Type);
        if (resType.Id == Type.NullId)
            Program.AnalyserFeedbacks.Add(new Feedback
            {
                Message =
                    $"This operation on {left} and {right} is either not supported or will be casted to another type at runtime",
                Severity = Feedback.SeverityType.Warning,
                SourceRange = SourceRange
            });

        var isConstant = left.IsConstant && right.IsConstant;
        var constantInstance = isConstant ? left.Type.BinaryOperators[OperatorMethod](left.ConstantInstance, right.ConstantInstance) : Instance.Null;
        if (isConstant)
        {
            Program.AnalyserFeedbacks.Add(new Feedback
                {
                    Message = $"Replace with constant {constantInstance}",
                    SourceRange = SourceRange,
                    Severity = Feedback.SeverityType.Hint,
                    Replacements = new List<Feedback.Replacement>
                    {
                        new()
                        {
                            SourceRange = SourceRange,
                            Text = constantInstance.Represent()
                        }
                    }
                });
        }

        Program.TypeCheckStack.Push(new TypeInfo {
            Type = resType,
            IsConstant = isConstant,
            SourceRange = SourceRange,
            ConstantInstance = constantInstance,
            IsConstantEvaluated = true
        });
    }

    public override string ToPlainString()
    {
        return string.Format(strings.BinaryOperation_ToPlainString, OperatorMethod);
    }
}
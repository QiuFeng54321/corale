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
        if (resType is NullType && left.Type.IsConvertableFrom(right.Type))
            resType = left.Type.BinaryResultType(OperatorMethod, left.Type);
        if (resType.Id == Type.NullId)
            Program.AnalyserFeedbacks.Add(new Feedback
            {
                Message =
                    $"This operation between {left} and {right} is not supported",
                Severity = Feedback.SeverityType.Error,
                SourceRange = SourceRange
            });

        var isConstant = left.Attributes.HasFlag(Definition.Attribute.Const) && right.Attributes.HasFlag(Definition.Attribute.Const);
        var constantInstance = isConstant
            ? left.Type.BinaryOperators[OperatorMethod](left.ConstantInstance, right.ConstantInstance)
            : Instance.Null;
        if (isConstant)
        {
            Program.AnalyserFeedbacks.Add(new Feedback
            {
                Message = $"Replace with constant {constantInstance}",
                SourceRange = SourceRange,
                Severity = Feedback.SeverityType.Hint,
                CodeFixes = new List<CodeFix>
                {
                    new()
                    {
                        Message = $"Replace with constant {constantInstance}",
                        Replacements = new List<CodeFix.Replacement>
                        {
                            new()
                            {
                                SourceRange = SourceRange,
                                Text = constantInstance.Represent()
                            }
                        }
                    }
                }
            });
        }

        Program.TypeCheckStack.Push(new Definition(ParentScope, Program)
        {
            Type = resType,
            SourceRange = SourceRange,
            ConstantInstance = constantInstance,
            Attributes = isConstant ? Definition.Attribute.Const : Definition.Attribute.Immutable
        });
    }

    public override string ToPlainString()
    {
        return string.Format(strings.BinaryOperation_ToPlainString, OperatorMethod);
    }
}
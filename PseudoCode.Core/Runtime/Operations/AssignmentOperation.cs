using PseudoCode.Core.Analyzing;
using PseudoCode.Core.Runtime.Types;
using Type = PseudoCode.Core.Runtime.Types.Type;

namespace PseudoCode.Core.Runtime.Operations;

public class AssignmentOperation : Operation
{
    public bool KeepVariableInStack;

    public AssignmentOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        var value = Program.RuntimeStack.Pop();
        var to = Program.RuntimeStack.Pop();
        var res = to.Type.Assign(to, value);
        if (KeepVariableInStack) Program.RuntimeStack.Push(res);
    }

    public override void MetaOperate()
    {
        base.MetaOperate();
        var value = Program.TypeCheckStack.Pop();
        TypeInfo to;
        try
        {
            to = Program.TypeCheckStack.Pop();
        }
        catch (InvalidOperationException e)
        {
            // incomplete expressions recognised as assignment
            Program.AnalyserFeedbacks.Add(new Feedback
            {
                Message = "Statement incomplete",
                Severity = Feedback.SeverityType.Error,
                SourceRange = SourceRange
            });
            return;
        }

        if (!to.IsReference)
        {
            Program.AnalyserFeedbacks.Add(new Feedback
            {
                Message = $"The target instance is not a reference",
                Severity = Feedback.SeverityType.Error,
                SourceRange = SourceRange
            });
        }
        if (to.Type is PlaceholderType placeholderType) to.Type = placeholderType.MetaAssign(value.Type);
        // TODO: Type check
        if (!to.Type.IsConvertableFrom(value.Type))
            Program.AnalyserFeedbacks.Add(new Feedback
            {
                Message = $"Cannot assign {value} to {to}",
                Severity = Feedback.SeverityType.Error,
                SourceRange = SourceRange
            });
        if (KeepVariableInStack) Program.TypeCheckStack.Push(to);
    }

    public override string ToPlainString()
    {
        return string.Format(strings.AssignmentOperation_ToPlainString,
            KeepVariableInStack ? strings.AssignmentOperation_ToPlainString_Keep : "");
    }
}
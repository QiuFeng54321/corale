using PseudoCode.Core.Analyzing;
using PseudoCode.Core.Runtime.Types;

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
        if (!Program.TypeCheckStack.TryPop(out var to))
        {
            // incomplete expressions recognised as assignment
            Program.AnalyserFeedbacks.Add(new Feedback
            {
                Message = "Statement incomplete",
                Severity = Feedback.SeverityType.Error,
                SourceRange = SourceRange
            });
            to = new Definition(ParentScope, Program)
            {
                Type = new NullType(ParentScope, Program),
                SourceRange = SourceRange,
                Name = "NULL",
                Attributes = Definition.Attribute.Reference
            };
            if (KeepVariableInStack) Program.TypeCheckStack.Push(to);
            return;
        }

        if (!to.Attributes.HasFlag(Definition.Attribute.Reference))
            Program.AnalyserFeedbacks.Add(new Feedback
            {
                Message = $"The assignment target is not a reference: {to}",
                Severity = Feedback.SeverityType.Error,
                SourceRange = SourceRange
            });

        if (to.Attributes.HasFlag(Definition.Attribute.Immutable))
            Program.AnalyserFeedbacks.Add(new Feedback
            {
                Message = $"Cannot assign value to a constant: {to}",
                Severity = Feedback.SeverityType.Error,
                SourceRange = SourceRange
            });

        if (to.Type is PlaceholderType placeholderType) to = placeholderType.MetaAssign(to, value);
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
using PseudoCode.Core.Analyzing;

namespace PseudoCode.Core.Runtime.Operations;

public class MakeConstOperation : DeclareOperation
{
    public MakeConstOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void MetaOperate()
    {
        var constDef = Program.TypeCheckStack.Pop();
        var type = constDef.Type;
        if (!constDef.Attributes.HasFlag(Definition.Attribute.Const))
        {
            Program.AnalyserFeedbacks.Add(new Feedback
            {
                Message = "Value is not a constant",
                Severity = Feedback.SeverityType.Error,
                SourceRange = SourceRange
            });
            Definition.Attributes = Definition.Attribute.Reference;
        }

        Definition.Type = type;
        Definition.ConstantInstance = constDef.ConstantInstance;
        base.MetaOperate();
    }
}
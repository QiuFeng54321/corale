using PseudoCode.Core.Analyzing;
using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Types;
using Type = PseudoCode.Core.Runtime.Types.Type;

namespace PseudoCode.Core.Runtime.Operations;

public class UnaryOperation : Operation
{
    public PseudoOperator OperatorMethod;

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
        var resType = to.Type.UnaryResultType(OperatorMethod);
        if (resType.Id == Type.NullId)
            Program.AnalyserFeedbacks.Add(new Feedback
            {
                Message =
                    $"{OperatorMethod} operation on {to} is either not supported or will be casted to another type at runtime",
                Severity = Feedback.SeverityType.Warning,
                SourceRange = SourceRange
            });
        var isConstant = to.Attributes.HasFlag(DefinitionAttribute.Const);
        Program.TypeCheckStack.Push(new Definition(ParentScope, Program)
        {
            Type = resType,
            Attributes = isConstant ? DefinitionAttribute.Const : DefinitionAttribute.Immutable,
            ConstantInstance = isConstant ? to.Type.UnaryOperators[OperatorMethod](to.ConstantInstance) : Instance.Null
        });
    }

    public override string ToPlainString()
    {
        return string.Format(strings.UnaryOperation_ToPlainString, OperatorMethod);
    }
}
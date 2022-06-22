using PseudoCode.Core.Analyzing;
using PseudoCode.Core.Runtime.Types;
using Type = PseudoCode.Core.Runtime.Types.Type;

namespace PseudoCode.Core.Runtime.Operations;

public class InputOperation : Operation
{
    public InputOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        var instance = Program.RuntimeStack.Pop();
        var input = Console.ReadLine();
        instance.Type.Assign(instance, Program.FindDefinition(Type.StringId).Type.Instance(input, ParentScope));
    }

    public override void MetaOperate()
    {
        base.MetaOperate();
        var valDef = Program.TypeCheckStack.Pop();
        var stringDef = Program.FindDefinition(Type.StringId);
        if (valDef.Type is PlaceholderType placeholderType) placeholderType.MetaAssign(valDef, stringDef);

        if (!valDef.Type.IsConvertableFrom(stringDef.Type))
            Program.AnalyserFeedbacks.Add(new Feedback
            {
                Message = $"INPUT variable is of type {valDef} and is not convertable from {stringDef}",
                Severity = Feedback.SeverityType.Error,
                SourceRange = SourceRange
            });
    }

    public override string ToPlainString()
    {
        return strings.InputOperation_ToPlainString;
    }
}
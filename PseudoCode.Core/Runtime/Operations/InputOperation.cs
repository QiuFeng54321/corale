using PseudoCode.Core.Analyzing;

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
        instance.Type.Assign(instance, ParentScope.FindTypeDefinition(Type.StringId).Type.Instance(input, ParentScope));
    }

    public override void MetaOperate()
    {
        base.MetaOperate();
        var type = Program.TypeCheckStack.Pop();
        var stringType = ParentScope.FindTypeDefinition(Type.StringId).Type;
        if (type is PlaceholderType placeholderType)
        {
            placeholderType.MetaAssign(stringType);
        }

        if (!type.IsConvertableFrom(stringType))
        {
            Program.AnalyserFeedbacks.Add(new Feedback
            {
                Message = $"INPUT variable is of type {type} and is not convertable from {stringType}",
                Severity = Feedback.SeverityType.Error,
                SourceRange = SourceRange
            });
        }
    }

    public override string ToPlainString()
    {
        return strings.InputOperation_ToPlainString;
    }
}
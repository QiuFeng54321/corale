using PseudoCode.Core.Analyzing;
using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Types;
using PseudoCode.Core.Runtime.Types.Descriptor;
using Type = PseudoCode.Core.Runtime.Types.Type;

namespace PseudoCode.Core.Runtime.Operations;

public class DeclareOperation : Operation
{
    public Definition Definition;

    public DeclareOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        // TODO: Initializing array within a type does not work properly
        var instance = Definition.ConstantInstance != null
            ? Definition.Type.Clone(Definition.ConstantInstance)
            : Definition.Type.Instance();
        if (instance is ArrayInstance arrayInstance)
        {
            var dimensionCount = ((ArrayDescriptor)Definition.TypeDescriptor).Dimensions;
            for (var i = 0; i < dimensionCount; i++)
            {
                var intType = Program.FindDefinition(Type.IntegerId).Type;
                var end = intType.CastFrom(Program.RuntimeStack.Pop());
                var start = intType.CastFrom(Program.RuntimeStack.Pop());
                var range = new Range { Start = start.Get<int>(), End = end.Get<int>() };
                arrayInstance.Dimensions.Insert(0, range);
            }

            arrayInstance.InitialiseInMemory();
        }

        ParentScope.ScopeStates.InstanceAddresses.Add(Definition.Name, Program.AllocateId(instance));
    }

    public override void MetaOperate()
    {
        base.MetaOperate();
        TypeCheck();
        ParentScope.AddVariableDefinition(Definition.Name, Definition, SourceRange);
    }

    protected void TypeCheck()
    {
        var invalidType = false;
        if (Definition.TypeDescriptor is ArrayDescriptor descriptor)
        {
            var dimensionCount = descriptor.Dimensions;
            for (var i = 0; i < dimensionCount; i++)
            {
                var intType = Program.FindDefinition(Type.IntegerId).Type;
                var invalidEnd = !intType.IsConvertableFrom(Program.TypeCheckStack.Pop().Type);
                var invalidStart = !intType.IsConvertableFrom(Program.TypeCheckStack.Pop().Type);
                invalidType |= invalidStart || invalidEnd;
            }
        }

        if (invalidType)
            Program.AnalyserFeedbacks.Add(new Feedback
            {
                Message = "Range value has expressions that does not evaluate or can not be casted to INTEGER",
                Severity = Feedback.SeverityType.Error,
                SourceRange = SourceRange
            });
        if (Definition.TypeDescriptor?.GetDefinition(ParentScope, Program) is { } definition &&
            !definition.Attributes.HasFlag(Definition.Attribute.Type))
            Program.AnalyserFeedbacks.Add(new Feedback
            {
                Message = $"Declared type should be a type, not '{Definition.TypeDescriptor.SelfName}'",
                Severity = Feedback.SeverityType.Error,
                SourceRange = SourceRange
            });
        switch (Definition.Type)
        {
            case null or NullType:
                Program.AnalyserFeedbacks.Add(new Feedback
                {
                    Message = $"The specified type does not exist: '{Definition.TypeName}'",
                    Severity = Feedback.SeverityType.Error,
                    SourceRange = SourceRange
                });
                break;
            case ArrayType { ElementType: null or NullType }:
                Program.AnalyserFeedbacks.Add(new Feedback
                {
                    Message =
                        $"The specified type does not exist: '{Definition?.TypeDescriptor?.ToString() ?? "NULL"}'",
                    Severity = Feedback.SeverityType.Error,
                    SourceRange = SourceRange
                });
                break;
            case AnyType:
                Program.AnalyserFeedbacks.Add(new Feedback
                {
                    Message =
                        "Type ANY is only used for type checking. It is not standard and is not intended to be usable.",
                    Severity = Feedback.SeverityType.Warning,
                    SourceRange = SourceRange
                });
                break;
        }
    }

    public override string ToPlainString()
    {
        var typeStr = Definition.ToString();
        return string.Format(strings.DeclareOperation_ToPlainString, Definition.Name, typeStr);
    }
}
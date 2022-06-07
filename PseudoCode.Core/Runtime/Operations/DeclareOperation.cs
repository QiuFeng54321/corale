using PseudoCode.Core.Analyzing;
using PseudoCode.Core.Runtime.Instances;
using Type = PseudoCode.Core.Runtime.Types.Type;

namespace PseudoCode.Core.Runtime.Operations;

public class DeclareOperation : Operation
{
    public Definition Definition;
    public int DimensionCount;
    public string Name;

    public DeclareOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        var instance = Definition.Type.Instance();
        if (instance is ArrayInstance arrayInstance)
        {
            for (var i = 0; i < DimensionCount; i++)
            {
                var intType = ParentScope.FindTypeDefinition(Type.IntegerId).Type;
                var end = intType.CastFrom(Program.RuntimeStack.Pop());
                var start = intType.CastFrom(Program.RuntimeStack.Pop());
                var range = new Range { Start = start.Get<int>(), End = end.Get<int>() };
                arrayInstance.Dimensions.Insert(0, range);
            }

            arrayInstance.InitialiseInMemory();
        }

        ParentScope.ScopeStates.InstanceAddresses.Add(Name, Program.AllocateId(instance));
    }

    public override void MetaOperate()
    {
        base.MetaOperate();
        var invalidType = false;
        for (var i = 0; i < DimensionCount; i++)
        {
            var intType = ParentScope.FindTypeDefinition(Type.IntegerId).Type;
            var invalidEnd = !intType.IsConvertableFrom(Program.TypeCheckStack.Pop().Type);
            var invalidStart = !intType.IsConvertableFrom(Program.TypeCheckStack.Pop().Type);
            invalidType |= invalidStart || invalidEnd;
        }

        if (invalidType)
            Program.AnalyserFeedbacks.Add(new Feedback
            {
                Message = "Range value has expressions that does not evaluate or can not be casted to INTEGER",
                Severity = Feedback.SeverityType.Error,
                SourceRange = SourceRange
            });
        ParentScope.AddVariableDefinition(Name, Definition, SourceRange);
    }

    public override string ToPlainString()
    {
        var typeStr = Definition.Type.ToString();
        return string.Format(strings.DeclareOperation_ToPlainString, Name, typeStr);
    }
}
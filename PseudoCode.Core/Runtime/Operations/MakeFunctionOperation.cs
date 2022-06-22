using PseudoCode.Core.Runtime.Errors;
using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.Runtime.Operations;

public class MakeFunctionOperation : Operation
{
    public Definition Definition;
    public Scope FunctionBody;
    public string Name;

    public MakeFunctionOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        var instance = Definition.Type.Instance();
        if (instance is not FunctionInstance functionInstance)
            throw new InvalidTypeError($"I'm making a function of {instance.Type}???", this);

        functionInstance.FunctionBody = FunctionBody;
        ParentScope.ScopeStates.InstanceAddresses.Add(Name, Program.AllocateId(functionInstance));
    }

    public override void MetaOperate()
    {
        base.MetaOperate();
        ParentScope.AddVariableDefinition(Name, Definition);
        foreach (var parameterInfo in ((FunctionType)Definition.Type).ParameterInfos)
            FunctionBody.AddVariableDefinition(parameterInfo.Name, parameterInfo);
        FunctionBody.MetaOperate();
    }

    public override string ToPlainString()
    {
        return $"Make function {Name}: {Definition.Type}";
    }

    public override string ToString(int depth)
    {
        return $"{Indent(depth)}{ToPlainString()}{FunctionBody.ToString(depth)}";
    }
}
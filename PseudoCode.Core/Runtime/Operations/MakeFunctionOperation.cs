using PseudoCode.Core.Runtime.Errors;

namespace PseudoCode.Core.Runtime.Operations;

public class MakeFunctionOperation : Operation
{
    public string Name;
    public Scope FunctionBody;
    public Definition Definition;
    public MakeFunctionOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        var instance = ParentScope.FindInstanceDefinition(Name).Type.Instance();
        if (instance is not FunctionInstance functionInstance)
        {
            throw new InvalidTypeError($"I'm making a function of {instance.Type}???", this);
        }

        functionInstance.FunctionBody = FunctionBody;
        ParentScope.ScopeStates.InstanceAddresses.Add(Name, Program.AllocateId(functionInstance));
    }

    public override void MetaOperate()
    {
        base.MetaOperate();
        ParentScope.AddVariableDefinition(Name, Definition);
    }

    public override string ToPlainString() => $"Make function {Name}: {Definition.Type}";
    public override string ToString(int depth)
    {
        return $"{Indent(depth)}{ToPlainString()}{FunctionBody.ToString(depth)}";
    }
}
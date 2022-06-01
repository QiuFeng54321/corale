using PseudoCode.Core.Runtime.Errors;

namespace PseudoCode.Core.Runtime.Operations;

public class DeclareOperation : Operation
{
    public string Name;

    public DeclareOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        var type = ParentScope.FindInstanceDefinition(Name);
        var instance = type.Type.Instance();
        if (instance is ArrayInstance arrayInstance)
        {
            arrayInstance.InitialiseInMemory();
        } 
        ParentScope.ScopeStates.InstanceAddresses.Add(Name, Program.AllocateId(instance));
    }

    public override void MetaOperate()
    {
        base.MetaOperate();
        
    }

    public override string ToPlainString()
    {
        var typeStr = ParentScope.FindInstanceDefinition(Name).ToString();
        return string.Format(strings.DeclareOperation_ToPlainString, Name, typeStr);
    }
}
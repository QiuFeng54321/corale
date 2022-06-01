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
        var type = ParentScope.FindInstanceType(Name);
        var instance = type.Instance();
        if (instance is ArrayInstance arrayInstance)
        {
            arrayInstance.InitialiseInMemory();
        } 
        ParentScope.ScopeStates.InstanceAddresses.Add(Name, Program.AllocateId(instance));
    }

    public override string ToPlainString()
    {
        var typeStr = ParentScope.FindInstanceType(Name).ToString();
        return string.Format(strings.DeclareOperation_ToPlainString, Name, typeStr);
    }
}
using PseudoCode.Runtime.Operations;

namespace PseudoCode.Runtime;

public class PlaceholderType : Type
{
    public override uint Id => PlaceholderId;
    public override string Name => "PLACEHOLDER";

    public override void Assign(Instance to, Instance value)
    {
        var placeholderInstance = (PlaceholderInstance)to;
        to.ParentScope.ScopeStates.InstanceAddresses.Add(placeholderInstance.Name, Program.AllocateId(value));
        // base.Assign(to, value);
    }
    public override Instance Instance(object value = null, Scope scope = null)
    {
        var instance = new PlaceholderInstance(scope ?? ParentScope, Program)
        {
            Type = this,
            Members = new Dictionary<string, Instance>(),
            Value = value
        };
        foreach (var member in Members) instance.Members[member.Key] = member.Value.Instance(scope: ParentScope);

        return instance;
    }
}
namespace PseudoCode.Runtime;

public class PlaceholderType : Type
{
    public override uint Id => PlaceholderId;
    public override string Name => "PLACEHOLDER";

    public override void Assign(Instance to, Instance value)
    {
        var placeholderInstance = (PlaceholderInstance)to;
        Scope.InstanceAddresses.Add(placeholderInstance.Name, Program.AllocateId(value));
        // base.Assign(to, value);
    }
    public override Instance Instance(object value = null)
    {
        var instance = new PlaceholderInstance(Scope, Program)
        {
            Type = this,
            Members = new Dictionary<string, Instance>(),
            Value = value
        };
        foreach (var member in Members) instance.Members[member.Key] = member.Value.Instance();

        return instance;
    }
}
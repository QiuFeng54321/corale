using System.Runtime.CompilerServices;
using PseudoCode.Runtime.Errors;
using PseudoCode.Runtime.Operations;

namespace PseudoCode.Runtime;

public class PlaceholderType : Type
{
    public override uint Id => PlaceholderId;
    public override string Name => "PLACEHOLDER";

    public override Instance Assign(Instance to, Instance value)
    {
        var placeholderInstance = (PlaceholderInstance)to;
        var allocatedId = Program.AllocateId(value.Type.Clone(value));
        to.ParentScope.ScopeStates.InstanceAddresses.Add(placeholderInstance.Name, allocatedId);
        return new ReferenceInstance(ParentScope, Program)
        {
            ReferenceAddress = allocatedId
        };
        // base.Assign(to, value);
    }

    public override Instance Clone(Instance instance)
    {
        throw MakeUnsupported(instance);
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

    public override Error MakeUnsupported(Instance i1, Instance i2 = null, [CallerMemberName] string caller = "Unknown")
    {
        throw new InvalidAccessError(string.Format(strings.Scope_FindInstanceAddress_NotFound, i1.Get<string>()), null)
        {
            PossibleCauses = new []
            {
                strings.PlaceholderType_ThrowUnsupported_PossibleCauses_AssignBeforeUse
            }
        };
    }
}
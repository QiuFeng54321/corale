using System.Runtime.CompilerServices;
using PseudoCode.Core.Runtime.Errors;
using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime.Types;

public class PlaceholderType : Type
{
    public string InstanceName;

    public PlaceholderType(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override uint Id => PlaceholderId;
    public override string Name => "PLACEHOLDER";

    public Definition MetaAssign(Definition thisDef, Definition valDef)
    {
        var resDef = thisDef with
        {
            Type = valDef.Type
        };
        ParentScope.InstanceDefinitions.Add(InstanceName, resDef);
        return resDef;
    }

    public override bool IsConvertableFrom(Type type)
    {
        return type.Id == StringId;
    }

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
        return DefaultInstance<PlaceholderInstance>(value, scope);
    }

    public override Error MakeUnsupported(Instance i1, Instance i2 = null, [CallerMemberName] string caller = "Unknown")
    {
        throw new InvalidAccessError(string.Format(strings.Scope_FindInstanceAddress_NotFound, i1.Get<string>()), null)
        {
            PossibleCauses = new[]
            {
                strings.PlaceholderType_ThrowUnsupported_PossibleCauses_AssignBeforeUse
            }
        };
    }
}
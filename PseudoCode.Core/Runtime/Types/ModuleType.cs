using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime.Types;

public class ModuleType : TypeType
{
    public ModuleType(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override string Name => "MODULE";

    public override Instance Instance(object value = null, Scope scope = null)
    {
        return DefaultInstance<ModuleInstance>();
    }
}
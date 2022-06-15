using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime.Types;

public class ModuleType : TypeType
{
    public override string Name => "MODULE";

    public ModuleType(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }
}
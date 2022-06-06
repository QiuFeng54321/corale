using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime.Types;

public class NullType : Type
{
    public override uint Id => NullId;
    public override string Name => "NULL";

    public NullType(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }
}
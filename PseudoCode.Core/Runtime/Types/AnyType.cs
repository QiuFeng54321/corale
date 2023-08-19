using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime.Types;

public class AnyType : Type
{
    public AnyType(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override uint Id => AnyId;
    public override string Name => "ANY";

    public override Instance Clone(Instance instance)
    {
        return instance.Type.Clone(instance);
    }

    public override Type BinaryResultType(PseudoOperator type, Type right)
    {
        return this;
    }

    public override Type UnaryResultType(PseudoOperator type)
    {
        return this;
    }

    public override bool IsConvertableFrom(Type type)
    {
        return true;
    }

    public override Instance CastFrom(Instance i)
    {
        return i;
    }
}
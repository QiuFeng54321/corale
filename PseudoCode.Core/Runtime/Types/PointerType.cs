using PseudoCode.Core.Runtime.Errors;
using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime.Types;

public class PointerType : Type
{
    public override string Name => "POINTER";
    public override uint Id => PointerId;
    public Type PointedType;

    public PointerType(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override bool IsConvertableFrom(Type type)
    {
        return type is PointerType;
    }

    public override Instance GetPointed(Instance i)
    {
        var address = (uint)i.Value;
        if (Program.Memory.TryGetValue(address, out var instance))
        {
            return PointedType.HandledCastFrom(instance);
        }

        throw new InvalidAccessError($"Access to uninitialized address {address}", null);
    }
}
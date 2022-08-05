using PseudoCode.Core.Runtime.Errors;
using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime.Types;

public class PointerType : PrimitiveType<uint>
{
    public override string Name => "POINTER";
    public override uint Id => PointerId;
    public Type PointedType;

    public PointerType(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override Type BinaryResultType(PseudoOperator type, Type right)
    {
        if (right is IntegerType)
        {
            return type switch
            {
                PseudoOperator.Add or PseudoOperator.Subtract => this,
                _ => base.BinaryResultType(type, right)
            };
        }
        return base.BinaryResultType(type, right);
    }

    public override Type UnaryResultType(PseudoOperator type)
    {
        return type is PseudoOperator.GetPointed ? PointedType : base.UnaryResultType(type);
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
    
    public override Instance CastFrom(Instance i)
    {
        return Instance(Convert.ToUInt32(i.Value), ParentScope);
    }
    public override Instance Add(Instance i1, Instance i2)
    {
        return ArithmeticOperation(i1, i2, (arg1, arg2) => arg1 + arg2);
    }

    public override Instance Subtract(Instance i1, Instance i2)
    {
        return ArithmeticOperation(i1, i2, (arg1, arg2) => arg1 - arg2);
    }
}
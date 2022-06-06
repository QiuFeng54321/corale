using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime.Types;

public class PrimitiveType<T> : Type
{
    public PrimitiveType(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override bool Serializable => true;

    public Instance ArithmeticOperation(Instance i1, Instance i2, Func<T, T, T> func)
    {
        // if (i2.Type.Id is not (IntegerId or RealId))
        //     throw new InvalidTypeError($"Invalid right operand type {i2.Type}", null);
        return Instance(func(HandledCastFrom(i1).Get<T>(), HandledCastFrom(i2).Get<T>()), ParentScope);
    }

    public Instance ArithmeticUnaryOperation(Instance i, Func<T, T> func)
    {
        return Instance(func(HandledCastFrom(i).Get<T>()), ParentScope);
    }

    public Instance LogicOperation(Instance i1, Instance i2, Func<T, T, bool> func)
    {
        return ParentScope.FindTypeDefinition(BooleanId).Type
            .Instance(func(HandledCastFrom(i1).Get<T>(), HandledCastFrom(i2).Get<T>()), ParentScope);
    }

    public Instance LogicUnaryOperation(Instance i, Func<T, bool> func)
    {
        return ParentScope.FindTypeDefinition(BooleanId).Type.Instance(func(HandledCastFrom(i).Get<T>()), ParentScope);
    }
}
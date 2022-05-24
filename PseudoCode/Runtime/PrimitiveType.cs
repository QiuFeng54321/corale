namespace PseudoCode.Runtime;

public class PrimitiveType<T> : Type
{
    public Instance ArithmeticOperation(Instance i1, Instance i2, Func<T, T, T> func)
    {
        if (i2.Type.Id is not (IntegerId or RealId))
            throw new InvalidOperationException($"Invalid right operand type {i2.Type}");
        return Instance(func(CastFrom(i1).Get<T>(), CastFrom(i2).Get<T>()));
    }
    public Instance ArithmeticUnaryOperation(Instance i, Func<T, T> func)
    {
        return Instance(func(CastFrom(i).Get<T>()));
    }

    public override Instance Instance(object value = null)
    {
        return new Instance { Type = this, Value = value };
    }
}
namespace PseudoCode.Runtime;

public class RealType : PrimitiveType<decimal>
{
    public override uint Id => RealId;
    public override string Name => "REAL";

    // public Instance ArithmeticOperation(Instance i1, Instance i2 , Func<decimal, decimal, decimal> func)
    // {
    //     if (i2.Type.Id is not (IntegerId or RealId))
    //         throw new InvalidOperationException($"Invalid right operand type {i2.Type}");
    //     return Instance(func(i1.Get<decimal>(), i2.Get<decimal>()));
    // }

    public override Instance Add(Instance i1, Instance i2)
    {
        return ArithmeticOperation(i1, i2, (arg1, arg2) => arg1 + arg2);
    }

    public override Instance Negative(Instance i)
    {
        return ArithmeticOperation(i, null!, (arg1, _) => -arg1);
    }

    public override Instance Subtract(Instance i1, Instance i2)
    {
        // if (i2.Type.Id is not (IntegerId or RealId))
        //     throw new InvalidOperationException($"Invalid right operand type {i2.Type}");
        // return Instance(i1.Get<decimal>() - i2.Get<decimal>());
        return ArithmeticOperation(i1, i2, (arg1, arg2) => arg1 - arg2);
        // return Add(i1, Negative(i2));
    }

    public override Instance CastFrom(Instance i)
    {
        return Instance(Convert.ToDecimal(i.Value));
    }
}
namespace PseudoCode.Core.Runtime;

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
        return ArithmeticUnaryOperation(i, arg => -arg);
    }

    public override Instance Subtract(Instance i1, Instance i2)
    {
        // if (i2.Type.Id is not (IntegerId or RealId))
        //     throw new InvalidOperationException($"Invalid right operand type {i2.Type}");
        // return Instance(i1.Get<decimal>() - i2.Get<decimal>());
        return ArithmeticOperation(i1, i2, (arg1, arg2) => arg1 - arg2);
        // return Add(i1, Negative(i2));
    }

    public override Instance Multiply(Instance i1, Instance i2)
    {
        return ArithmeticOperation(i1, i2, (arg1, arg2) => arg1 * arg2);
    }

    public override Instance Divide(Instance i1, Instance i2)
    {
        return ArithmeticOperation(i1, i2, (arg1, arg2) => arg1 / arg2);
    }

    public override Instance Mod(Instance i1, Instance i2)
    {
        return ArithmeticOperation(i1, i2, (arg1, arg2) => arg1 % arg2);
    }

    public override Instance Pow(Instance i1, Instance i2)
    {
        return ArithmeticOperation(i1, i2, (arg1, arg2) => (decimal)Math.Pow((double)arg1, (double)arg2));
    }

    public override Instance IntDivide(Instance i1, Instance i2)
    {
        return ParentScope.FindType(IntegerId).HandledCastFrom(
            ArithmeticOperation(i1, i2, (arg1, arg2) => (int)(arg1 / arg2)));
    }

    public override Instance Greater(Instance i1, Instance i2)
    {
        return LogicOperation(i1, i2, (arg1, arg2) => arg1 > arg2);
    }

    public override Instance GreaterEqual(Instance i1, Instance i2)
    {
        return LogicOperation(i1, i2, (arg1, arg2) => arg1 >= arg2);
    }

    public override Instance Smaller(Instance i1, Instance i2)
    {
        return LogicOperation(i1, i2, (arg1, arg2) => arg1 < arg2);
    }

    public override Instance SmallerEqual(Instance i1, Instance i2)
    {
        return LogicOperation(i1, i2, (arg1, arg2) => arg1 <= arg2);
    }

    public override Instance Equal(Instance i1, Instance i2)
    {
        return LogicOperation(i1, i2, (arg1, arg2) => arg1 == arg2);
    }

    public override Instance NotEqual(Instance i1, Instance i2)
    {
        return LogicOperation(i1, i2, (arg1, arg2) => arg1 != arg2);
    }

    public override Instance CastFrom(Instance i)
    {
        return Instance(Convert.ToDecimal(i.Value), ParentScope);
    }
}
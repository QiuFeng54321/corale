namespace PseudoCode.Runtime;

public class IntegerType : PrimitiveType<int>
{
    public override uint Id => IntegerId;
    public override string Name => "INTEGER";


    public override Instance Add(Instance i1, Instance i2)
    {
        return i2.Type == this
            ? ArithmeticOperation(i1, i2, (arg1, arg2) => arg1 + arg2)
            : Scope.FindType(RealId).Add(i1, i2);
    }

    public override Instance Subtract(Instance i1, Instance i2)
    {
        return i2.Type == this
            ? ArithmeticOperation(i1, i2, (arg1, arg2) => arg1 - arg2)
            : Scope.FindType(RealId).Subtract(i1, i2);
    }

    public override Instance Negative(Instance i)
    {
        return ArithmeticOperation(i, null, (arg1, _) => -arg1);
    }

    public override Instance Multiply(Instance i1, Instance i2)
    {
        return i2.Type == this
            ? ArithmeticOperation(i1, i2, (arg1, arg2) => arg1 * arg2)
            : Scope.FindType(RealId).Multiply(i1, i2);
    }

    public override Instance Divide(Instance i1, Instance i2)
    {
        return Scope.FindType(RealId).Divide(i1, i2);
    }

    public override Instance Mod(Instance i1, Instance i2)
    {
        return i2.Type == this
            ? ArithmeticOperation(i1, i2, (arg1, arg2) => arg1 % arg2)
            : Scope.FindType(RealId).Mod(i1, i2);
    }

    public override Instance Pow(Instance i1, Instance i2)
    {
        return Scope.FindType(RealId).Pow(i1, i2);
    }

    public override Instance IntDivide(Instance i1, Instance i2)
    {
        return ArithmeticOperation(i1, i2, (arg1, arg2) => arg1 / arg2);
    }

    public override Instance Greater(Instance i1, Instance i2)
    {
        return Scope.FindType(RealId).Greater(i1, i2);
    }

    public override Instance GreaterEqual(Instance i1, Instance i2)
    {
        return Scope.FindType(RealId).GreaterEqual(i1, i2);
    }

    public override Instance Smaller(Instance i1, Instance i2)
    {
        return Scope.FindType(RealId).Smaller(i1, i2);
    }

    public override Instance SmallerEqual(Instance i1, Instance i2)
    {
        return Scope.FindType(RealId).SmallerEqual(i1, i2);
    }

    public override Instance Equal(Instance i1, Instance i2)
    {
        return Scope.FindType(RealId).Equal(i1, i2);
    }

    public override Instance NotEqual(Instance i1, Instance i2)
    {
        return Scope.FindType(RealId).NotEqual(i1, i2);
    }

    public override Instance CastFrom(Instance i)
    {
        return Instance(Convert.ToInt32(i.Value));
    }

    public Instance CastToReal(Instance i)
    {
        return Scope.FindType(RealId).HandledCastFrom(i);
    }
}
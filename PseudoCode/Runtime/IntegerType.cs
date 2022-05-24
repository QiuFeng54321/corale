namespace PseudoCode.Runtime;

public class IntegerType : PrimitiveType<int>
{
    public override uint Id => IntegerId;
    public override string Name => "INTEGER";


    public override Instance Add(Instance i1, Instance i2)
    {
        return i2.Type == this
            ? ArithmeticOperation(i1, i2, (arg1, arg2) => arg1 + arg2)
            : ParentScope.FindType(RealId).Add(i1, i2);
    }
    public override Instance Subtract(Instance i1, Instance i2)
    {
        return i2.Type == this
            ? ArithmeticOperation(i1, i2, (arg1, arg2) => arg1 - arg2)
            : ParentScope.FindType(RealId).Subtract(i1, i2);
    }
    public override Instance Negative(Instance i)
    {
        return ArithmeticOperation(i, null, (arg1, _) => -arg1);
    }
    
    public override Instance Multiply(Instance i1, Instance i2)
    {
        return i2.Type == this
            ? ArithmeticOperation(i1, i2, (arg1, arg2) => arg1 * arg2)
            : ParentScope.FindType(RealId).Multiply(i1, i2);
    }
    
    public override Instance Divide(Instance i1, Instance i2)
    {
        return ParentScope.FindType(RealId).Divide(i1, i2);
    }
    public override Instance Mod(Instance i1, Instance i2)
    {
        return i2.Type == this
            ? ArithmeticOperation(i1, i2, (arg1, arg2) => arg1 % arg2)
            : ParentScope.FindType(RealId).Mod(i1, i2);
    }
    public override Instance Pow(Instance i1, Instance i2)
    {
        return ParentScope.FindType(RealId).Pow(i1, i2);
    }
    
    public override Instance IntDivide(Instance i1, Instance i2)
    {
        return ArithmeticOperation(i1, i2, (arg1, arg2) => arg1 / arg2);
    }
    public override Instance CastFrom(Instance i)
    {
        return Instance(Convert.ToInt32(i.Value));
    }

    public Instance CastToReal(Instance i)
    {
        return ParentScope.FindType(RealId).CastFrom(i);
    }
}
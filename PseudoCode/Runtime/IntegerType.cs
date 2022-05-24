namespace PseudoCode.Runtime;

public class IntegerType : PrimitiveType<int>
{
    public override uint Id => IntegerId;
    public override string Name => "INTEGER";


    public override Instance Add(Instance i1, Instance i2)
    {
        return i2.Type == this
            ? ArithmeticOperation(i1, i2, (arg1, arg2) => arg1 + arg2)
            : ParentScope.FindType(RealId).Add(CastToReal(i1), CastToReal(i2));
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
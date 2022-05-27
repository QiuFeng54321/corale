namespace PseudoCode.Runtime;

public class BooleanType : PrimitiveType<bool>
{
    public override uint Id => BooleanId;
    public override string Name => "BOOLEAN";


    public override Instance And(Instance i1, Instance i2)
    {
        return LogicOperation(i1, i2, (arg1, arg2) => arg1 && arg2);
    }

    public override Instance Or(Instance i1, Instance i2)
    {
        return LogicOperation(i1, i2, (arg1, arg2) => arg1 || arg2);
    }

    public override Instance Not(Instance i)
    {
        return LogicUnaryOperation(i, arg1 => !arg1);
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
        return Instance(Convert.ToBoolean(i.Value));
    }
}
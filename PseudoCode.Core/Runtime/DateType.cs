using System.Globalization;

namespace PseudoCode.Core.Runtime;

public class DateType : PrimitiveType<DateOnly>
{
    public override uint Id => DateId;
    public override string Name => "DATE";

    // public Instance ArithmeticOperation(Instance i1, Instance i2 , Func<decimal, decimal, decimal> func)
    // {
    //     if (i2.Type.Id is not (IntegerId or RealId))
    //         throw new InvalidOperationException($"Invalid right operand type {i2.Type}");
    //     return Instance(func(i1.Get<decimal>(), i2.Get<decimal>()));
    // }

    public override Instance Add(Instance i1, Instance i2)
    {
        return ArithmeticOperation(i1, i2, AddDate);
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

    public DateOnly AddDate(DateOnly d1, DateOnly d2)
    {
        return new DateOnly(d1.Year, d1.Month, d1.Day).AddDays(d2.Day).AddMonths(d2.Month).AddYears(d2.Year);
    }

    public override Instance CastFrom(Instance i)
    {
        return i.Type.Id switch
        {
            DateId => i,
            StringId => Instance(DateOnly.ParseExact(i.Get<string>(), "dd/MM/yyyy"), ParentScope),
            _ => base.CastFrom(i)
        };
    }
}

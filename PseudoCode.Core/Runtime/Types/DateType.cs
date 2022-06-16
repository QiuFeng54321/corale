using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime.Types;

public class DateType : PrimitiveType<DateOnly>
{
    public DateType(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override uint Id => DateId;
    public override string Name => "DATE";

    // public Instance ArithmeticOperation(Instance i1, Instance i2 , Func<decimal, decimal, decimal> func)
    // {
    //     if (i2.Type.Id is not (IntegerId or RealId))
    //         throw new InvalidOperationException($"Invalid right operand type {i2.Type}");
    //     return Instance(func(i1.Get<decimal>(), i2.Get<decimal>()));
    // }
    public override Type BinaryResultType(int type, Type right)
    {
        if (right is not DateType) return new NullType(ParentScope, Program);
        switch (type)
        {
            case PseudoCodeLexer.Equal:
            case PseudoCodeLexer.NotEqual:
            case PseudoCodeLexer.Greater:
            case PseudoCodeLexer.GreaterEqual:
            case PseudoCodeLexer.Smaller:
            case PseudoCodeLexer.SmallerEqual:
                return Program.FindDefinition(BooleanId).Type;
            case PseudoCodeLexer.Add:
                return this;
            default:
                return new NullType(ParentScope, Program);
        }
    }

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

    public override bool IsConvertableFrom(Type type)
    {
        return type.Id is DateId or StringId;
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
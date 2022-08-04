global using RealNumberType = System.Decimal;
using System.Diagnostics.CodeAnalysis;
using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime.Types;

public class RealType : PrimitiveType<RealNumberType>
{
    public RealType(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override uint Id => RealId;
    public override string Name => "REAL";

    // public Instance ArithmeticOperation(Instance i1, Instance i2 , Func<RealNumberType, RealNumberType, RealNumberType> func)
    // {
    //     if (i2.Type.Id is not (IntegerId or RealId))
    //         throw new InvalidOperationException($"Invalid right operand type {i2.Type}");
    //     return Instance(func(i1.Get<RealNumberType>(), i2.Get<RealNumberType>()));
    // }

    public override Type BinaryResultType(PseudoOperator type, Type right)
    {
        if (right is not (IntegerType or RealType)) return new NullType(ParentScope, Program);

        switch (type)
        {
            case PseudoOperator.Equal:
            case PseudoOperator.NotEqual:
            case PseudoOperator.Greater:
            case PseudoOperator.GreaterEqual:
            case PseudoOperator.Smaller:
            case PseudoOperator.SmallerEqual:
                return Program.FindDefinition(BooleanId).Type;
            case PseudoOperator.IntDivide:
                return Program.FindDefinition(IntegerId).Type;
            case PseudoOperator.And:
            case PseudoOperator.BitAnd:
            case PseudoOperator.Or:
                return new NullType(ParentScope, Program);
            default:
                return this;
        }
    }

    public override Type UnaryResultType(PseudoOperator type)
    {
        return type == PseudoOperator.Not ? Program.FindDefinition(BooleanId).Type : this;
    }

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
        // return Instance(i1.Get<RealNumberType>() - i2.Get<RealNumberType>());
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

    [SuppressMessage("ReSharper", "RedundantCast")]
    public override Instance Pow(Instance i1, Instance i2)
    {
        return ArithmeticOperation(i1, i2, (arg1, arg2) => (RealNumberType)Math.Pow((double)arg1, (double)arg2));
    }

    public override Instance IntDivide(Instance i1, Instance i2)
    {
        return Program.FindDefinition(IntegerId).Type.HandledCastFrom(
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
        return Instance(Convert.ChangeType(i.Value, typeof(RealNumberType)), ParentScope);
    }

    public override bool IsConvertableFrom(Type type)
    {
        switch (type)
        {
            case IntegerType:
            case BooleanType:
            case StringType:
            case CharacterType:
            case RealType:
            case EnumType:
                return true;
            default:
                return false;
        }
    }
}
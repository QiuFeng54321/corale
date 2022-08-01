using System.Diagnostics.CodeAnalysis;
using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime.Types;

public class EnumType : PrimitiveType<int>
{
    public List<string> Names = new();
    public EnumType(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }
    public override Type BinaryResultType(int type, Type right)
    {
        if (right is not (IntegerType or RealType or EnumType)) return new NullType(ParentScope, Program);

        switch (type)
        {
            case PseudoCodeLexer.Equal:
            case PseudoCodeLexer.NotEqual:
            case PseudoCodeLexer.Greater:
            case PseudoCodeLexer.GreaterEqual:
            case PseudoCodeLexer.Smaller:
            case PseudoCodeLexer.SmallerEqual:
                return Program.FindDefinition(BooleanId).Type;
            case PseudoCodeLexer.IntDivide:
                return Program.FindDefinition(IntegerId).Type;
            case PseudoCodeLexer.And:
            case PseudoCodeLexer.BitAnd:
            case PseudoCodeLexer.Or:
                return new NullType(ParentScope, Program);
            default:
                return this;
        }
    }

    public override Type UnaryResultType(int type)
    {
        return type == PseudoCodeLexer.Not ? Program.FindDefinition(BooleanId).Type : this;
    }

    public override Instance Add(Instance i1, Instance i2)
    {
        return ArithmeticOperation(i1, i2, (arg1, arg2) => arg1 + arg2);
    }

    public override Instance Subtract(Instance i1, Instance i2)
    {
        // if (i2.Type.Id is not (IntegerId or RealId))
        //     throw new InvalidOperationException($"Invalid right operand type {i2.Type}");
        // return Instance(i1.Get<RealNumberType>() - i2.Get<RealNumberType>());
        return ArithmeticOperation(i1, i2, (arg1, arg2) => arg1 - arg2);
        // return Add(i1, Negative(i2));
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
        return Instance(Convert.ChangeType(i.Value, typeof(int)), ParentScope);
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
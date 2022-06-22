using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime.Types;

public class IntegerType : PrimitiveType<int>
{
    public IntegerType(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override uint Id => IntegerId;
    public override string Name => "INTEGER";

    public override Type BinaryResultType(int type, Type right)
    {
        if (right is not (IntegerType or RealType)) return new NullType(ParentScope, Program);
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
                return right;
        }
    }

    public override Type UnaryResultType(int type)
    {
        return type == PseudoCodeLexer.Not ? Program.FindDefinition(BooleanId).Type : this;
    }

    public override Instance Add(Instance i1, Instance i2)
    {
        return i2.Type == this
            ? ArithmeticOperation(i1, i2, (arg1, arg2) => arg1 + arg2)
            : Program.FindDefinition(RealId).Type.Add(i1, i2);
    }

    public override Instance Subtract(Instance i1, Instance i2)
    {
        return i2.Type == this
            ? ArithmeticOperation(i1, i2, (arg1, arg2) => arg1 - arg2)
            : Program.FindDefinition(RealId).Type.Subtract(i1, i2);
    }

    public override Instance Negative(Instance i)
    {
        return ArithmeticOperation(i, null, (arg1, _) => -arg1);
    }

    public override Instance Multiply(Instance i1, Instance i2)
    {
        return i2.Type == this
            ? ArithmeticOperation(i1, i2, (arg1, arg2) => arg1 * arg2)
            : Program.FindDefinition(RealId).Type.Multiply(i1, i2);
    }

    public override Instance Divide(Instance i1, Instance i2)
    {
        return Program.FindDefinition(RealId).Type.Divide(i1, i2);
    }

    public override Instance Mod(Instance i1, Instance i2)
    {
        return i2.Type == this
            ? ArithmeticOperation(i1, i2, (arg1, arg2) => arg1 % arg2)
            : Program.FindDefinition(RealId).Type.Mod(i1, i2);
    }

    public override Instance Pow(Instance i1, Instance i2)
    {
        return Program.FindDefinition(RealId).Type.Pow(i1, i2);
    }

    public override Instance IntDivide(Instance i1, Instance i2)
    {
        return ArithmeticOperation(i1, i2, (arg1, arg2) => arg1 / arg2);
    }

    public override Instance Greater(Instance i1, Instance i2)
    {
        return Program.FindDefinition(RealId).Type.Greater(i1, i2);
    }

    public override Instance GreaterEqual(Instance i1, Instance i2)
    {
        return Program.FindDefinition(RealId).Type.GreaterEqual(i1, i2);
    }

    public override Instance Smaller(Instance i1, Instance i2)
    {
        return Program.FindDefinition(RealId).Type.Smaller(i1, i2);
    }

    public override Instance SmallerEqual(Instance i1, Instance i2)
    {
        return Program.FindDefinition(RealId).Type.SmallerEqual(i1, i2);
    }

    public override Instance Equal(Instance i1, Instance i2)
    {
        return Program.FindDefinition(RealId).Type.Equal(i1, i2);
    }

    public override Instance NotEqual(Instance i1, Instance i2)
    {
        return Program.FindDefinition(RealId).Type.NotEqual(i1, i2);
    }

    public override Instance CastFrom(Instance i)
    {
        return Instance(Convert.ToInt32(i.Value), ParentScope);
    }

    public Instance CastToReal(Instance i)
    {
        return Program.FindDefinition(RealId).Type.HandledCastFrom(i);
    }

    public override bool IsConvertableFrom(Type type)
    {
        switch (type.Id)
        {
            case IntegerId:
            case BooleanId:
            case StringId:
            case CharId:
            case RealId:
                return true;
            default:
                return false;
        }
    }
}
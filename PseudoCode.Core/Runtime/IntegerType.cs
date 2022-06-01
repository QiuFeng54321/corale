using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime;

public class IntegerType : PrimitiveType<int>
{
    public override uint Id => IntegerId;
    public override string Name => "INTEGER";
    public override Type BinaryResultType(int type, Type right)
    {
        if (right is not (IntegerType or RealType))
        {
            return null;
        }
        switch (type)
        {
            case PseudoCodeLexer.Equal:
            case PseudoCodeLexer.NotEqual:
            case PseudoCodeLexer.Greater:
            case PseudoCodeLexer.GreaterEqual:
            case PseudoCodeLexer.Smaller:
            case PseudoCodeLexer.SmallerEqual:
                return ParentScope.FindTypeDefinition(BooleanId).Type;
            case PseudoCodeLexer.IntDivide:
                return ParentScope.FindTypeDefinition(IntegerId).Type;
            case PseudoCodeLexer.And:
            case PseudoCodeLexer.BitAnd:
            case PseudoCodeLexer.Or:
                return null;
            default:
                return right;
        }
    }

    public override Type UnaryResultType(int type)
    {
        return type == PseudoCodeLexer.Not ? ParentScope.FindTypeDefinition(BooleanId).Type : this;
    }

    public override Instance Add(Instance i1, Instance i2)
    {
        return i2.Type == this
            ? ArithmeticOperation(i1, i2, (arg1, arg2) => arg1 + arg2)
            : ParentScope.FindTypeDefinition(RealId).Type.Add(i1, i2);
    }

    public override Instance Subtract(Instance i1, Instance i2)
    {
        return i2.Type == this
            ? ArithmeticOperation(i1, i2, (arg1, arg2) => arg1 - arg2)
            : ParentScope.FindTypeDefinition(RealId).Type.Subtract(i1, i2);
    }

    public override Instance Negative(Instance i)
    {
        return ArithmeticOperation(i, null, (arg1, _) => -arg1);
    }

    public override Instance Multiply(Instance i1, Instance i2)
    {
        return i2.Type == this
            ? ArithmeticOperation(i1, i2, (arg1, arg2) => arg1 * arg2)
            : ParentScope.FindTypeDefinition(RealId).Type.Multiply(i1, i2);
    }

    public override Instance Divide(Instance i1, Instance i2)
    {
        return ParentScope.FindTypeDefinition(RealId).Type.Divide(i1, i2);
    }

    public override Instance Mod(Instance i1, Instance i2)
    {
        return i2.Type == this
            ? ArithmeticOperation(i1, i2, (arg1, arg2) => arg1 % arg2)
            : ParentScope.FindTypeDefinition(RealId).Type.Mod(i1, i2);
    }

    public override Instance Pow(Instance i1, Instance i2)
    {
        return ParentScope.FindTypeDefinition(RealId).Type.Pow(i1, i2);
    }

    public override Instance IntDivide(Instance i1, Instance i2)
    {
        return ArithmeticOperation(i1, i2, (arg1, arg2) => arg1 / arg2);
    }

    public override Instance Greater(Instance i1, Instance i2)
    {
        return ParentScope.FindTypeDefinition(RealId).Type.Greater(i1, i2);
    }

    public override Instance GreaterEqual(Instance i1, Instance i2)
    {
        return ParentScope.FindTypeDefinition(RealId).Type.GreaterEqual(i1, i2);
    }

    public override Instance Smaller(Instance i1, Instance i2)
    {
        return ParentScope.FindTypeDefinition(RealId).Type.Smaller(i1, i2);
    }

    public override Instance SmallerEqual(Instance i1, Instance i2)
    {
        return ParentScope.FindTypeDefinition(RealId).Type.SmallerEqual(i1, i2);
    }

    public override Instance Equal(Instance i1, Instance i2)
    {
        return ParentScope.FindTypeDefinition(RealId).Type.Equal(i1, i2);
    }

    public override Instance NotEqual(Instance i1, Instance i2)
    {
        return ParentScope.FindTypeDefinition(RealId).Type.NotEqual(i1, i2);
    }

    public override Instance CastFrom(Instance i)
    {
        return Instance(Convert.ToInt32(i.Value), ParentScope);
    }

    public Instance CastToReal(Instance i)
    {
        return ParentScope.FindTypeDefinition(RealId).Type.HandledCastFrom(i);
    }

    public IntegerType(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }
}
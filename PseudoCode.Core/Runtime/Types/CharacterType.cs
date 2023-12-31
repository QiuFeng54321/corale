using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime.Types;

public class CharacterType : PrimitiveType<char>
{
    public CharacterType(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override uint Id => CharId;
    public override string Name => "CHAR";

    public override Type BinaryResultType(PseudoOperator type, Type right)
    {
        if (right is not (IntegerType or RealType or CharacterType)) return new NullType(ParentScope, Program);
        switch (type)
        {
            case PseudoOperator.Equal:
            case PseudoOperator.NotEqual:
            case PseudoOperator.Greater:
            case PseudoOperator.GreaterEqual:
            case PseudoOperator.Smaller:
            case PseudoOperator.SmallerEqual:
                return Program.FindDefinition(BooleanId).Type;
            default:
                return new NullType(ParentScope, Program);
        }
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

    public override bool IsConvertableFrom(Type type)
    {
        switch (type.Id)
        {
            case IntegerId:
            case BooleanId:
            case StringId:
            case RealId:
            case CharId:
                return true;
            default:
                return false;
        }
    }

    public override Instance CastFrom(Instance i)
    {
        return Instance(Convert.ToChar(i.Value), ParentScope);
    }
}
using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime;

public class CharacterType : PrimitiveType<char>
{
    public override uint Id => CharId;
    public override string Name => "CHAR";
    public override Type BinaryResultType(int type, Type right)
    {
        if (right is not (IntegerType or RealType or CharacterType))
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
            default:
                return null;
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

    public CharacterType(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }
}
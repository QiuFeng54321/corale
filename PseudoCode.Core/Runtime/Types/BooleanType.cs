using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime.Types;

public class BooleanType : PrimitiveType<bool>
{
    public override uint Id => BooleanId;
    public override string Name => "BOOLEAN";
    public override Type BinaryResultType(int type, Type right)
    {
        if (right is not BooleanType)
        {
            return new NullType(ParentScope, Program);
        }
        switch (type)
        {
            case PseudoCodeLexer.And:
            case PseudoCodeLexer.Or:
            case PseudoCodeLexer.Equal:
            case PseudoCodeLexer.NotEqual:
                return this;
            default:
                return new NullType(ParentScope, Program);
        }
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
    public override Type UnaryResultType(int type)
    {
        return type == PseudoCodeLexer.Not ? this : null;
    }

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
        return Instance(Convert.ToBoolean(i.Value), ParentScope);
    }

    public BooleanType(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }
}
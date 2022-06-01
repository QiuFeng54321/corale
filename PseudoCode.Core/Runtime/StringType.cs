using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime;

public class StringType : PrimitiveType<string>
{
    public override uint Id => StringId;
    public override string Name => "STRING";

    public override Type BinaryResultType(int type, Type right)
    {
        switch (type)
        {
            case PseudoCodeLexer.Equal:
            case PseudoCodeLexer.NotEqual:
                return ParentScope.FindType(BooleanId);
            case PseudoCodeLexer.BitAnd:
                return this;
            default:
                return null;
        }
    }
    
    public override Instance Equal(Instance i1, Instance i2)
    {
        return LogicOperation(i1, i2, (arg1, arg2) => arg1 == arg2);
    }

    public override Instance NotEqual(Instance i1, Instance i2)
    {
        return LogicOperation(i1, i2, (arg1, arg2) => arg1 != arg2);
    }

    public override Instance BitAnd(Instance i1, Instance i2)
    {
        return Instance(i1.ToString() + i2, ParentScope);
    }

    public override Instance CastFrom(Instance i)
    {
        return Instance(Convert.ToString(i.Value), ParentScope);
    }

    public StringType(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }
}
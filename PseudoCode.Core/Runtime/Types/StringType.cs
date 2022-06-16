using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime.Types;

public class StringType : PrimitiveType<string>
{
    public StringType(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override uint Id => StringId;
    public override string Name => "STRING";

    public override Type BinaryResultType(int type, Type right)
    {
        switch (type)
        {
            case PseudoCodeLexer.Equal:
            case PseudoCodeLexer.NotEqual:
                return Program.FindDefinition(BooleanId).Type;
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
        return Instance(i1.Represent() + i2.Represent(), ParentScope);
    }

    public override bool IsConvertableFrom(Type type)
    {
        return true;
    }

    public override Instance CastFrom(Instance i)
    {
        return Instance(Convert.ToString(i.Value), ParentScope);
    }
}
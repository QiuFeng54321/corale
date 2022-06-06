using PseudoCode.Core.Analyzing;
using Type = PseudoCode.Core.Runtime.Types.Type;

namespace PseudoCode.Core.Runtime.Operations;

public class RepeatOperation : Operation
{
    public Operation RepeatBlock;

    public RepeatOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        var test = true;
        while (test)
        {
            RepeatBlock.HandledOperate();
            test = ParentScope.FindTypeDefinition(Type.BooleanId).Type.HandledCastFrom(Program.RuntimeStack.Pop()).Get<bool>();
        }
    }

    public override void MetaOperate()
    {
        base.MetaOperate();
        RepeatBlock.MetaOperate();
    }

    public override string ToPlainString() => "Repeat-Until";

    public override string ToString(int depth)
    {
        return string.Format(strings.RepeatOperation_ToString_Repeat, Indent(depth), RepeatBlock.ToString(depth), Indent(depth));
    }
}
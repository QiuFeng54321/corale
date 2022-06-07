using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.Runtime.Operations;

public class LoadImmediateOperation : Operation
{
    public Instance Intermediate;

    public LoadImmediateOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        Program.RuntimeStack.Push(Intermediate);
    }

    public override void MetaOperate()
    {
        base.MetaOperate();
        Program.TypeCheckStack.Push(new TypeInfo {
            Type = Intermediate.Type,
            IsConstant = true,
            ConstantInstance = Intermediate,
            SourceRange = SourceRange
        });
    }

    public override string ToPlainString()
    {
        return string.Format(strings.LoadImmediateOperation_ToPlainString, Intermediate);
    }
}
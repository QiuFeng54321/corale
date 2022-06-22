using PseudoCode.Core.Runtime.Instances;

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
        Program.TypeCheckStack.Push(new Definition(ParentScope, Program)
        {
            Type = Intermediate.Type,
            ConstantInstance = Intermediate,
            SourceRange = SourceRange,
            Attributes = Definition.Attribute.Const
        });
    }

    public override string ToPlainString()
    {
        return string.Format(strings.LoadImmediateOperation_ToPlainString, Intermediate);
    }
}
using PseudoCode.Core.Analyzing;
using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.Formatting;

namespace PseudoCode.Core.CodeGen;

public class AssignmentStatement : Statement
{
    public Expression Target, Value;

    public override void CodeGen(CodeGenContext ctx, Block block)
    {
        var val = Value.CodeGen(ctx, block).GetRealValueRef(ctx);
        var target = Target.CodeGen(ctx, block).MemoryPointer;
        if (target == null)
            ctx.Analysis.Feedbacks.Add(new Feedback
            {
                Severity = Feedback.SeverityType.Error,
                Message = $"{Value} not assignable"
            });
        ctx.Builder.BuildStore(val, target);
    }

    public override void Format(PseudoFormatter formatter)
    {
        formatter.WriteStatement($"{Target.ToFormatString()} <- {Value.ToFormatString()}");
    }
}
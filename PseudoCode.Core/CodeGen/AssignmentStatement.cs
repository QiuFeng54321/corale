using PseudoCode.Core.Analyzing;
using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.Formatting;

namespace PseudoCode.Core.CodeGen;

public class AssignmentStatement : Statement
{
    public Expression Target, Value;

    public override void CodeGen(CodeGenContext ctx, CompilationUnit cu, Function function)
    {
        var val = Value.CodeGen(ctx, cu, function).GetRealValueRef(ctx, cu);
        var target = Target.CodeGen(ctx, cu, function).GetPointerValueRef();
        if (target == null)
            ctx.Analysis.Feedbacks.Add(new Feedback
            {
                Severity = Feedback.SeverityType.Error,
                Message = $"{Value} not assignable"
            });
        cu.Builder.BuildStore(val, target);
    }

    public override void Format(PseudoFormatter formatter)
    {
        formatter.WriteStatement($"{Target.ToFormatString()} <- {Value.ToFormatString()}");
    }
}
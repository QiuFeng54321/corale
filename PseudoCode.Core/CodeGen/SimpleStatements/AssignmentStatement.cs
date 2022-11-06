using PseudoCode.Core.Analyzing;
using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.Formatting;

namespace PseudoCode.Core.CodeGen.SimpleStatements;

public class AssignmentStatement : Statement
{
    public Expression Target, Value;

    public override void CodeGen(CodeGenContext ctx, CompilationUnit cu, Function function)
    {
        var valueSym = Value.CodeGen(ctx, cu, function);
        var targetSym = Target.CodeGen(ctx, cu, function);
        if (targetSym.Type != valueSym.Type)
        {
            var castedValueSym = ctx.OperatorResolverMap.CasterMap.Cast(valueSym, targetSym, ctx, cu, function);
            if (castedValueSym.IsError)
            {
                ctx.Analysis.Feedbacks.Add(new Feedback
                {
                    Message = $"Cannot assign {valueSym.GetTypeString()} to {targetSym.GetTypeString()}",
                    DebugInformation = DebugInformation,
                    Severity = Feedback.SeverityType.Error
                });
                return;
            }

            valueSym = castedValueSym;
        }

        var val = valueSym.GetRealValueRef(ctx, cu);
        var target = targetSym.GetPointerValueRef(ctx);
        if (target == null)
        {
            ctx.Analysis.Feedbacks.Add(new Feedback
            {
                Severity = Feedback.SeverityType.Error,
                Message = $"\"{Target}\" is not assignable because it's not a reference.",
                DebugInformation = DebugInformation
            });
            return;
        }

        cu.Builder.BuildStore(val, target);
    }

    public override void Format(PseudoFormatter formatter)
    {
        formatter.WriteStatement($"{Target.ToFormatString()} <- {Value.ToFormatString()}");
    }
}
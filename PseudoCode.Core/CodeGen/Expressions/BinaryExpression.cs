using PseudoCode.Core.Analyzing;
using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.CodeGen.Expressions;

public class BinaryExpression : Expression
{
    public Expression Left, Right;
    public PseudoOperator Operator;

    public override Symbol CodeGen(CodeGenContext ctx, CompilationUnit cu, Function function)
    {
        var left = Left.CodeGen(ctx, cu, function);
        var right = Right?.CodeGen(ctx, cu, function);
        if (left.IsError || (right != null && right.IsError)) return DebugInformation.MakeErrorSymbol();
        var res = ctx.OperatorResolverMap.Resolve(left, right, Operator, ctx, cu);
        if (res == null)
        {
            ctx.Analysis.Feedbacks.Add(new Feedback
            {
                Message = $"Operator {Operator} not implemented for {left.GetTypeString()}" +
                          (right != null ? $" and {right.GetTypeString()}" : ""),
                Severity = Feedback.SeverityType.Error,
                DebugInformation = DebugInformation
            });
            return DebugInformation.MakeErrorSymbol();
        }

        return res;
    }

    public override string ToFormatString()
    {
        if (Right == null)
            return $"{Operator.ToFormattedString()}{Left.ToFormatString()}";
        return $"{Left.ToFormatString()} {Operator.ToFormattedString()} {Right.ToFormatString()}";
    }
}
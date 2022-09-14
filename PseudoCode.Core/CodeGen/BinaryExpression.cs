using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.CodeGen;

public class BinaryExpression : Expression
{
    public Expression Left, Right;
    public PseudoOperator Operator;

    public override Symbol CodeGen(CodeGenContext ctx, Block block)
    {
        var left = Left.CodeGen(ctx, block);
        var right = Right?.CodeGen(ctx, block);
        return ctx.OperatorResolverMap.Resolve(left, right, Operator, ctx);
    }

    public override string Format()
    {
        if (Right == null)
            return $"{Operator} {Left}";
        return $"{Left} {Operator} {Right}";
    }
}
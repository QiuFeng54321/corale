namespace PseudoCode.Core.CodeGen;

public class ParenthesisExpression : Expression
{
    public Expression IncludingExpression;

    public ParenthesisExpression(Expression includingExpression)
    {
        IncludingExpression = includingExpression;
    }

    public override Symbol CodeGen(CodeGenContext ctx, Block block)
    {
        return IncludingExpression.CodeGen(ctx, block);
    }

    public override string ToFormatString()
    {
        return $"({IncludingExpression.ToFormatString()})";
    }
}
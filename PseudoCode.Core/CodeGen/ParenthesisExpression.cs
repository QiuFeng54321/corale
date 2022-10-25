using PseudoCode.Core.CodeGen.Containers;

namespace PseudoCode.Core.CodeGen;

public class ParenthesisExpression : Expression
{
    public readonly Expression IncludingExpression;

    public ParenthesisExpression(Expression includingExpression)
    {
        IncludingExpression = includingExpression;
    }

    public override Symbol CodeGen(CodeGenContext ctx, Function function)
    {
        return IncludingExpression.CodeGen(ctx, function);
    }

    public override string ToFormatString()
    {
        return $"({IncludingExpression.ToFormatString()})";
    }
}
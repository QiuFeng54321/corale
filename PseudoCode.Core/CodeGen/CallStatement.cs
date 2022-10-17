using PseudoCode.Core.Formatting;

namespace PseudoCode.Core.CodeGen;

public class CallStatement : Statement
{
    public Expression Expression;

    public override void Format(PseudoFormatter formatter)
    {
        formatter.WriteStatement($"CALL {Expression.ToFormatString()}");
    }

    public override void CodeGen(CodeGenContext ctx, Block block)
    {
        Symbol returnSymbol = null;
        if (Expression is not CallExpression)
        {
            var implicitCallExpr = new CallExpression
            {
                Function = Expression,
                Arguments = new List<Expression>()
            };
            returnSymbol = implicitCallExpr.CodeGen(ctx, block);
        }
        else
        {
            returnSymbol = Expression.CodeGen(ctx, block);
        }
    }
}
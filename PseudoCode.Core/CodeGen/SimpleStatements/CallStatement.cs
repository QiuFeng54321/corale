using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.CodeGen.Expressions;
using PseudoCode.Core.Formatting;

namespace PseudoCode.Core.CodeGen.SimpleStatements;

public class CallStatement : Statement
{
    public Expression Expression;

    public override void Format(PseudoFormatter formatter)
    {
        formatter.WriteStatement($"CALL {Expression.ToFormatString()}");
    }

    public override void CodeGen(CodeGenContext ctx, CompilationUnit cu, Function function)
    {
        if (Expression is not CallExpression)
        {
            var implicitCallExpr = new CallExpression
            {
                Function = Expression,
                Arguments = new List<Expression>()
            };
            implicitCallExpr.CodeGen(ctx, cu, function);
        }
        else
        {
            Expression.CodeGen(ctx, cu, function);
        }
    }
}
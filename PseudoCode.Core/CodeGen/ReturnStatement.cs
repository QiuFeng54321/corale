using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.Formatting;

namespace PseudoCode.Core.CodeGen;

public class ReturnStatement : Statement
{
    public Expression Expression;

    public override void Format(PseudoFormatter formatter)
    {
        formatter.WriteStatement($"RETURN {Expression.ToFormatString()}");
    }

    public override void CodeGen(CodeGenContext ctx, Function function)
    {
        ctx.Builder.BuildRet(Expression.CodeGen(ctx, function).GetRealValueRef(ctx));
    }
}
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
        var returnSym = Expression.CodeGen(ctx, function);
        ctx.Builder.BuildRet(function.ReturnType.DefinitionAttribute.HasFlag(DefinitionAttribute.Reference)
            ? returnSym.GetPointerValueRef()
            : returnSym.GetRealValueRef(ctx));
    }
}
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

    public override void CodeGen(CodeGenContext ctx, CompilationUnit cu, Function function)
    {
        var returnSym = Expression.CodeGen(ctx, cu, function);
        cu.Builder.BuildRet(function.ReturnType.DefinitionAttribute.HasFlag(DefinitionAttribute.Reference)
            ? returnSym.GetPointerValueRef(ctx)
            : returnSym.GetRealValueRef(ctx, cu));
    }
}
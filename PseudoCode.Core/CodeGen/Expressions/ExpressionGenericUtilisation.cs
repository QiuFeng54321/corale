using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.CodeGen.TypeLookups;

namespace PseudoCode.Core.CodeGen.Expressions;

public class ExpressionGenericUtilisation : Expression
{
    public Expression Expression;
    public GenericUtilisation GenericUtilisation;

    public override Symbol CodeGen(CodeGenContext ctx, CompilationUnit cu, Function function)
    {
        var sym = Expression.CodeGen(ctx, cu, function);
        var filledSym = sym.FillGeneric(ctx, cu, function, GenericUtilisation.GetSymbols(ctx, cu, function));
        return filledSym;
    }

    public override string ToFormatString()
    {
        return Expression.ToFormatString() + GenericUtilisation;
    }
}
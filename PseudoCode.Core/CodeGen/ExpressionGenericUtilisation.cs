using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.CodeGen.TypeLookups;

namespace PseudoCode.Core.CodeGen;

public class ExpressionGenericUtilisation : Expression
{
    public Expression Expression;
    public GenericUtilisation GenericUtilisation;

    public override Symbol CodeGen(CodeGenContext ctx, Function function)
    {
        var sym = Expression.CodeGen(ctx, function);
        var filledSym = sym.FillGeneric(ctx, function, GenericUtilisation.GetSymbols(ctx, function));
        return filledSym;
    }

    public override string ToFormatString()
    {
        return Expression.ToFormatString() + GenericUtilisation;
    }
}
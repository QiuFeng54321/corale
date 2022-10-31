using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.CodeGen.TypeLookups;

namespace PseudoCode.Core.CodeGen.Expressions;

public class LoadExpr : Expression
{
    public NamespaceLookup NamespaceLookup;

    public override Symbol CodeGen(CodeGenContext ctx, CompilationUnit cu, Function function)
    {
        return NamespaceLookup.Lookup(ctx, function);
    }

    public override string ToFormatString()
    {
        return NamespaceLookup.ToString();
    }
}
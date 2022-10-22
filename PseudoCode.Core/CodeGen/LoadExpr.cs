using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.Runtime.Errors;

namespace PseudoCode.Core.CodeGen;

public class LoadExpr : Expression
{
    public string Name;

    public override Symbol CodeGen(CodeGenContext ctx, Function function)
    {
        if (function.BodyNamespace.TryGetSymbol(Name, out var sym)) return sym;
        throw new InvalidAccessError(Name);
    }

    public override string ToFormatString()
    {
        return Name;
    }
}
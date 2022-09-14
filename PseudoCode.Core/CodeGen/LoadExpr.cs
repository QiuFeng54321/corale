using PseudoCode.Core.Runtime.Errors;

namespace PseudoCode.Core.CodeGen;

public class LoadExpr : Expression
{
    public string Name;

    public override Symbol CodeGen(CodeGenContext ctx, Block block)
    {
        if (block.Namespace.TryGetSymbol(Name, out var sym)) return sym;
        throw new InvalidAccessError(Name);
    }

    public override string Format()
    {
        return Name;
    }
}
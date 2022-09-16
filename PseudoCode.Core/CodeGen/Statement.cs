using PseudoCode.Core.Formatting;

namespace PseudoCode.Core.CodeGen;

public abstract class Statement : AstNode, IPseudoFormattable
{
    public abstract void Format(PseudoFormatter formatter);
    public abstract void CodeGen(CodeGenContext ctx, Block block);
}
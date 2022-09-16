namespace PseudoCode.Core.CodeGen;

public abstract class Expression : AstNode
{
    public abstract Symbol CodeGen(CodeGenContext ctx, Block block);

    public abstract string ToFormatString();
}
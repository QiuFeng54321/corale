namespace PseudoCode.Core.CodeGen;

public abstract class Statement : AstNode
{
    public abstract void CodeGen(CodeGenContext ctx, Block block);
}
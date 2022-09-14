namespace PseudoCode.Core.CodeGen;

public class DeclarationStatement : Statement
{
    public Symbol Symbol;

    public override void CodeGen(CodeGenContext ctx, Block block)
    {
        Symbol.MemoryPointer = ctx.Builder.BuildAlloca(Symbol.Type.GetLLVMType(), Symbol.Name);
    }
}
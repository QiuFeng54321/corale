using LLVMSharp.Interop;

namespace PseudoCode.Core.CodeGen;

public class Block : AstNode
{
    public LLVMBasicBlockRef BlockRef;
    public string Name;
    public Namespace Namespace;
    public List<Statement> Statements = new();


    public LLVMBasicBlockRef GetBlock(CodeGenContext ctx, Block parent)
    {
        // if (BlockRef != null) return BlockRef;
        unsafe
        {
            BlockRef = LLVM.AppendBasicBlock(parent.BlockRef, PseudoString.ToSByte(Name));
        }

        ctx.Builder.PositionAtEnd(BlockRef);
        Statements.ForEach(s => s.CodeGen(ctx, this));
        return BlockRef;
    }
}
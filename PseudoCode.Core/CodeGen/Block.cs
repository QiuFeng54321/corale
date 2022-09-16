using LLVMSharp.Interop;
using PseudoCode.Core.Formatting;

namespace PseudoCode.Core.CodeGen;

public class Block : AstNode, IPseudoFormattable
{
    public LLVMBasicBlockRef BlockRef;
    public string Name;
    public Namespace Namespace;
    public List<Statement> Statements = new();

    public virtual void Format(PseudoFormatter formatter)
    {
        formatter.Indent();
        WriteStatements(formatter);
        formatter.Dedent();
    }


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

    protected void WriteStatements(PseudoFormatter formatter)
    {
        foreach (var statement in Statements) statement.Format(formatter);
    }

    public override string ToString()
    {
        using var strWriter = new StringWriter();
        using var formatter = new PseudoFormatter(strWriter);
        Format(new PseudoFormatter(strWriter));
        return strWriter.ToString();
    }
}
using LLVMSharp.Interop;
using PseudoCode.Core.Formatting;

namespace PseudoCode.Core.CodeGen.Containers;

public class Block : Statement
{
    public LLVMBasicBlockRef BlockRef;
    public string Name;
    public Namespace Namespace;
    public Block ParentBlock;
    public Function ParentFunction;
    public List<Statement> Statements = new();

    public override void Format(PseudoFormatter formatter)
    {
        formatter.Indent();
        WriteStatements(formatter);
        formatter.Dedent();
    }

    /// <summary>
    ///     Creates a child block.
    /// </summary>
    /// <param name="name">Name of the sub-block</param>
    /// <param name="ns">The namespace of the sub-block</param>
    /// <param name="dangling">If the sub-block will be added to Statements</param>
    /// <returns>The created block</returns>
    public Block EnterBlock(string name, Namespace ns = null, bool dangling = false)
    {
        var block = new Block();
        if (!dangling) Statements.Add(block);
        block.ParentBlock = this;
        block.ParentFunction = ParentFunction;
        block.Namespace = ns ?? Namespace;
        block.Name = name;
        return block;
    }

    public override void CodeGen(CodeGenContext ctx, Block _)
    {
        GetBlock(ctx);
    }

    public unsafe LLVMBasicBlockRef InitializeBlock()
    {
        if (BlockRef != null) return BlockRef;
        var function = ParentFunction.LLVMFunction;
        BlockRef = LLVM.AppendBasicBlock(function, Name.ToSByte());
        return BlockRef;
    }

    public LLVMBasicBlockRef GetBlock(CodeGenContext ctx)
    {
        // if (BlockRef != null) return BlockRef;
        if (BlockRef == null)
        {
            InitializeBlock();
        }

        ctx.Builder.PositionAtEnd(BlockRef);
        CodeGenDirectly(ctx);
        return BlockRef;
    }

    public void CodeGenDirectly(CodeGenContext ctx)
    {
        foreach (var s in Statements) s.CodeGen(ctx, this);
    }

    protected void WriteStatements(PseudoFormatter formatter)
    {
        foreach (var statement in Statements) statement.Format(formatter);
    }
}
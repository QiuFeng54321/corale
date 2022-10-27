using PseudoCode.Core.Formatting;

namespace PseudoCode.Core.CodeGen.Containers;

/// <summary>
///     A block is a scope and is NOT an LLVMBasicBlockRef!!
/// </summary>
public class Block : Statement
{
    public readonly List<Statement> Statements = new();
    private Function _parentFunction;
    public string Name;
    public Namespace Namespace;
    public Block ParentBlock;

    public Function ParentFunction
    {
        get => _parentFunction ?? ParentBlock?.ParentFunction;
        set => _parentFunction = value;
    }

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
        block.Namespace = ns ?? Namespace;
        block.Name = name;
        return block;
    }

    public override void CodeGen(CodeGenContext ctx, CompilationUnit cu, Function _)
    {
        CodeGenDirectly(ctx, cu);
    }

    private void CodeGenDirectly(CodeGenContext ctx, CompilationUnit cu)
    {
        foreach (var s in Statements) s.CodeGen(ctx, cu, ParentFunction);
    }

    private void WriteStatements(PseudoFormatter formatter)
    {
        foreach (var statement in Statements) statement.Format(formatter);
    }
}
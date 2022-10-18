using LLVMSharp.Interop;
using PseudoCode.Core.Formatting;

namespace PseudoCode.Core.CodeGen.Containers;

public class Function : Statement
{
    public readonly List<Block> Blocks = new();
    public List<Symbol> Arguments;
    public CompilationUnit CompilationUnit;
    public LLVMValueRef LLVMFunction;
    public string Name;
    public Symbol ReturnType;

    public unsafe void GeneratePrototype(CodeGenContext ctx)
    {
        LLVMFunction = LLVM.AddFunction(ctx.Module, Name.ToSByte(),
            LLVMTypeRef.CreateFunction(ReturnType.Type.GetLLVMType(),
                Arguments.Select(a => a.Type.GetLLVMType()).ToArray()));
        for (uint index = 0; index < Arguments.Count; index++)
        {
            var argument = Arguments[(int)index];
            LLVM.SetValueName(LLVM.GetParam(LLVMFunction, index), argument.Name.ToSByte());
        }

        var entry = AddBlock("entry");
        ctx.Builder.PositionAtEnd(entry.BlockRef);
    }

    public Block AddBlock(string name, Namespace ns = null)
    {
        var block = new Block
        {
            Name = name,
            Namespace = ns ?? CompilationUnit.Namespace,
            ParentFunction = this
        };
        block.InitializeBlock();
        Blocks.Add(block);
        return block;
    }

    public override void Format(PseudoFormatter formatter)
    {
        formatter.WriteStatement(
            $"FUNCTION {Name}({string.Join(", ", Arguments.Select(a => $"{a.Name} : {a.Type.TypeName}"))}) RETURNS {ReturnType?.Type.TypeName}");

        foreach (var block in Blocks) block.Format(formatter);
        formatter.WriteStatement("ENDFUNCTION");
    }

    public override void CodeGen(CodeGenContext ctx, Block _)
    {
        if (LLVMFunction == null) GeneratePrototype(ctx);
        foreach (var block1 in Blocks) block1.CodeGen(ctx, _);

        ctx.Builder.BuildRetVoid();
    }
}
using PseudoCode.Core.CodeGen.TypeLookups;

namespace PseudoCode.Core.CodeGen;

public class DeclarationStatement : Statement
{
    public DataType DataType;
    public string Name;

    public override void CodeGen(CodeGenContext ctx, Block block)
    {
        var symbol = new Symbol(Name, false, DataType.Lookup(ctx, block).Type);
        block.Namespace.AddSymbol(symbol);
        symbol.MemoryPointer = ctx.Builder.BuildAlloca(symbol.Type.GetLLVMType(), symbol.Name);
    }

    public override string Format()
    {
        return $"DECLARE {Name} : {DataType}";
    }
}
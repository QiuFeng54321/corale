using PseudoCode.Core.CodeGen.TypeLookups;
using PseudoCode.Core.Formatting;

namespace PseudoCode.Core.CodeGen;

public class DeclarationStatement : Statement
{
    public DataType DataType;
    public string Name;

    public Symbol GetTypeSymbol(CodeGenContext ctx, Block block, Type parentType = default)
    {
        return DataType.Lookup(ctx, block);
    }

    public override void CodeGen(CodeGenContext ctx, Block block)
    {
        var typeSymbol = GetTypeSymbol(ctx, block);
        var symbol = MakeSymbol(typeSymbol);
        block.Namespace.AddSymbol(symbol);
        symbol.MemoryPointer = ctx.Builder.BuildAlloca(typeSymbol.Type.GetLLVMType(), symbol.Name);
    }

    public Symbol MakeSymbol(Symbol typeSymbol)
    {
        return new Symbol(Name, false, typeSymbol.Type);
    }

    public override void Format(PseudoFormatter formatter)
    {
        formatter.WriteStatement($"DECLARE {Name} : {DataType}");
    }
}
using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.CodeGen.TypeLookups;
using PseudoCode.Core.Formatting;

namespace PseudoCode.Core.CodeGen;

public class DeclarationStatement : Statement
{
    public DataType DataType;
    public string Name;

    public Symbol GetTypeSymbol(CodeGenContext ctx, Function function, Namespace ns = default)
    {
        return DataType.Lookup(ctx, function, ns);
    }

    public override void CodeGen(CodeGenContext ctx, Function function)
    {
        var typeSymbol = GetTypeSymbol(ctx, function);
        var symbol = MakeSymbol(typeSymbol);
        function.BodyNamespace.AddSymbol(symbol);
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
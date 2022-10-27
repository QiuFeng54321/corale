using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.CodeGen.TypeLookups;
using PseudoCode.Core.Formatting;

namespace PseudoCode.Core.CodeGen;

public class DeclarationStatement : Statement
{
    public DataType DataType;
    public string Name;

    public Symbol GetTypeSymbol(CodeGenContext ctx, CompilationUnit cu, Function function, Namespace ns = default)
    {
        return DataType.Lookup(ctx, cu, function, ns);
    }

    public override void CodeGen(CodeGenContext ctx, CompilationUnit cu, Function function)
    {
        var typeSymbol = GetTypeSymbol(ctx, cu, function);
        var symbol = typeSymbol.MakeStructMemberDeclSymbol(Name);
        function.BodyNamespace.AddSymbol(symbol);
        symbol.MemoryPointer = cu.Builder.BuildAlloca(typeSymbol.Type.GetLLVMType(), symbol.Name);
    }

    public override void Format(PseudoFormatter formatter)
    {
        formatter.WriteStatement($"DECLARE {Name} : {DataType}");
    }
}
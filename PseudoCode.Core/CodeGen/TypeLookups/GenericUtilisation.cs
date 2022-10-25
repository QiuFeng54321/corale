using PseudoCode.Core.CodeGen.Containers;

namespace PseudoCode.Core.CodeGen.TypeLookups;

public class GenericUtilisation
{
    public readonly List<DataType> TypeParameters;

    public GenericUtilisation(List<DataType> typeParameters)
    {
        TypeParameters = typeParameters;
    }

    public List<Symbol> GetSymbols(CodeGenContext ctx, Function function, Namespace ns = default)
    {
        return TypeParameters.Select(t => t.Lookup(ctx, function, ns)).ToList();
    }

    public override string ToString()
    {
        return TypeParameters != null ? $"<{string.Join(", ", TypeParameters)}>" : "";
    }
}
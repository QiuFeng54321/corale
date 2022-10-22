using PseudoCode.Core.CodeGen.Containers;

namespace PseudoCode.Core.CodeGen.TypeLookups;

public class DataType
{
    private readonly ModularType _modularType;

    public DataType(ModularType modularType)
    {
        _modularType = modularType;
    }

    public Symbol Lookup(CodeGenContext ctx, Function function, Namespace ns = default)
    {
        return _modularType.Lookup(ctx, function, ns ?? function.BodyNamespace);
    }

    public override string ToString()
    {
        return _modularType.ToString();
    }
}
using PseudoCode.Core.CodeGen.Containers;

namespace PseudoCode.Core.CodeGen.TypeLookups;

public class DataType
{
    // For pointer and array type
    private readonly DataType _elementType;
    private readonly ModularType _modularType;

    public DataType(ModularType modularType)
    {
        _modularType = modularType;
    }

    public DataType(DataType elementType)
    {
        _elementType = elementType;
    }

    public Symbol Lookup(CodeGenContext ctx, Function function, Namespace ns = default)
    {
        if (_modularType != null)
            return _modularType.Lookup(ctx, function, ns ?? function.BodyNamespace);
        return _elementType.Lookup(ctx, function, ns).MakePointerType();
    }

    public override string ToString()
    {
        if (_modularType != null) return _modularType.ToString();
        return "^" + _elementType;
    }
}
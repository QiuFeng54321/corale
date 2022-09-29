namespace PseudoCode.Core.CodeGen.TypeLookups;

public class DataType
{
    private readonly ModularType _modularType;

    public DataType(ModularType modularType)
    {
        _modularType = modularType;
    }

    public Symbol Lookup(CodeGenContext ctx, Block block, Type parentType = default)
    {
        return _modularType.Lookup(ctx, block, parentType);
    }

    public override string ToString()
    {
        return _modularType.ToString();
    }
}
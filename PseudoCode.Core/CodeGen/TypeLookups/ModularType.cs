using PseudoCode.Core.CodeGen.Containers;

namespace PseudoCode.Core.CodeGen.TypeLookups;

public class ModularType
{
    private readonly List<DataType> _genericParameters;
    private readonly TypeLookup _typeLookup;

    public ModularType(TypeLookup typeLookup, List<DataType> genericParameters = default)
    {
        _typeLookup = typeLookup;
        _genericParameters = genericParameters;
    }

    public Symbol Lookup(CodeGenContext ctx, Block block)
    {
        var symbol = _typeLookup.Lookup(block).Symbol;
        if (_genericParameters != null)
            symbol = symbol.FillGeneric(ctx, block,
                _genericParameters.Select(t => t.Lookup(ctx, block)).ToList());

        return symbol;
    }

    public override string ToString()
    {
        return _typeLookup + (_genericParameters != null ? $"<{string.Join(", ", _genericParameters)}>" : "");
    }
}
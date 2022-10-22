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

    public Symbol Lookup(CodeGenContext ctx, Function function, Namespace ns)
    {
        var symbol = _typeLookup.Lookup(ns).Symbol;
        if (_genericParameters != null)
            symbol = symbol.FillGeneric(ctx, function,
                _genericParameters.Select(t => t.Lookup(ctx, function, ns)).ToList());

        return symbol;
    }

    public override string ToString()
    {
        return _typeLookup + (_genericParameters != null ? $"<{string.Join(", ", _genericParameters)}>" : "");
    }
}
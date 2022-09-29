using PseudoCode.Core.Runtime.Errors;

namespace PseudoCode.Core.CodeGen.TypeLookups;

public class TypeLookup
{
    private readonly string _name;
    private readonly TypeLookup _parent;

    public TypeLookup(string name, TypeLookup parent = default)
    {
        _name = name;
        _parent = parent;
    }

    public SymbolOrNamespace Lookup(Block block, Type parentType = default)
    {
        if (parentType?.GenericArguments != null && parentType.GenericArguments.Any(s => s.Name == _name))
            return new SymbolOrNamespace(Symbol.MakeTypeSymbol(Type.MakeGenericPlaceholder(_name)));
        var ns = _parent?.Lookup(block, parentType).Ns ?? block.Namespace;
        if (ns.TryGetNamespace(_name, out var nsFound)) return new SymbolOrNamespace(Ns: nsFound);

        if (ns.TryGetSymbol(_name, out var sym)) return new SymbolOrNamespace(sym);
        throw new InvalidAccessError($"Unknown namespace or symbol: {_name}");
    }

    public override string ToString()
    {
        return _parent == null ? _name : $"{_parent}.{_name}";
    }
}
using PseudoCode.Core.CodeGen.Containers;
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

    public SymbolOrNamespace Lookup(Block block)
    {
        // 找到
        var ns = _parent?.Lookup(block).Ns ?? block.Namespace;
        if (ns.TryGetSymbol(_name, out var sym)) return new SymbolOrNamespace(sym);
        if (ns.TryGetNamespace(_name, out var nsFound)) return new SymbolOrNamespace(Ns: nsFound);

        throw new InvalidAccessError($"Unknown namespace or symbol: {_name}");
    }

    public override string ToString()
    {
        return _parent == null ? _name : $"{_parent}.{_name}";
    }
}
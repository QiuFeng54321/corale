using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.Runtime.Errors;

namespace PseudoCode.Core.CodeGen.TypeLookups;

public class NamespaceLookup
{
    public readonly string Identifier;
    public readonly NamespaceLookup ParentNs;

    public NamespaceLookup(string identifier, NamespaceLookup parentNs = null)
    {
        Identifier = identifier;
        ParentNs = parentNs;
    }

    public Namespace LookupNs(Function function)
    {
        if (ParentNs is null)
        {
            if (function.BodyNamespace.TryGetNamespace(Identifier, out var rtNs)) return rtNs;

            throw new InvalidAccessError(ToString());
        }

        if (ParentNs.LookupNs(function).TryGetNamespace(Identifier, out var ns)) return ns;

        throw new InvalidAccessError(ToString());
    }

    public Symbol Lookup(Function function)
    {
        var ns = ParentNs?.LookupNs(function) ?? function.BodyNamespace;
        if (ns.TryGetSymbol(Identifier, out var sym)) return sym;

        throw new InvalidAccessError(ToString());
    }

    public override string ToString()
    {
        return ParentNs == null ? Identifier : $"{ParentNs}.{Identifier}";
    }
}
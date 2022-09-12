namespace PseudoCode.Core.CodeGen;

public class Namespace
{
    /// <summary>
    ///     Member namespaces.
    /// </summary>
    public Dictionary<string, Namespace> ChildrenNamespaces = new();

    /// <summary>
    ///     The name of this namespace
    /// </summary>
    public string Name;

    /// <summary>
    ///     The parent namespace. Null if this namespace is root
    /// </summary>
    public Namespace Parent;

    /// <summary>
    ///     Stores the symbols defined under this namespace
    /// </summary>
    public Dictionary<string, Symbol> Symbols = new();

    public Namespace(string name, Namespace parent)
    {
        Name = name;
        Parent = parent;
    }

    public bool TryGetSymbol(string name, out Symbol result)
    {
        return Symbols.TryGetValue(name, out result);
    }

    public void AddSymbol(Symbol symbol)
    {
        Symbols.Add(symbol.Name, symbol);
    }

    public Namespace AddNamespace(string name)
    {
        var ns = new Namespace(name, this);
        ChildrenNamespaces.Add(name, ns);
        return ns;
    }

    public override string ToString()
    {
        return Parent == null ? "" : $"{Parent}.{Name}";
    }
}
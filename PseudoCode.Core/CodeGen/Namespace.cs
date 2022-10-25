namespace PseudoCode.Core.CodeGen;

public class Namespace
{
    /// <summary>
    ///     Member namespaces.
    /// </summary>
    public readonly Dictionary<string, Namespace> ChildrenNamespaces = new();

    /// <summary>
    ///     The name of this namespace
    /// </summary>
    public readonly string Name;

    /// <summary>
    ///     The parent namespace. Null if this namespace is root
    /// </summary>
    public readonly Namespace Parent;

    /// <summary>
    ///     Stores the symbols defined under this namespace
    /// </summary>
    public readonly Dictionary<string, Symbol> Symbols = new();

    public Namespace(string name, Namespace parent)
    {
        Name = name;
        Parent = parent;
    }

    public bool TryGetSymbol(string name, out Symbol result)
    {
        if (!Symbols.TryGetValue(name, out result)) return Parent != null && Parent.TryGetSymbol(name, out result);
        return true;
    }

    public bool TryGetNamespace(string name, out Namespace result)
    {
        if (!ChildrenNamespaces.TryGetValue(name, out result))
            return Parent != null && Parent.TryGetNamespace(name, out result);
        return true;
    }

    public void AddSymbol(Symbol symbol, bool setNs = true, string name = default)
    {
        Symbols.Add(name ?? symbol.Name, symbol);
        if (setNs) symbol.Namespace = this;
    }

    public Namespace AddNamespace(string name)
    {
        var ns = new Namespace(name, this);
        ChildrenNamespaces.Add(name, ns);
        return ns;
    }

    public string GetFullQualifier(string typeName)
    {
        return Parent == null ? typeName : $"{ToString()}.{typeName}";
    }

    public override string ToString()
    {
        return Parent == null ? "" : $"{Parent}.{Name}";
    }
}
namespace PseudoCode.Core.CodeGen;

public static class HelperFunctions
{
    public static Dictionary<string, Type> Clone(this Dictionary<string, Type> source)
    {
        return source.ToDictionary(p => p.Key, p => p.Value.Clone());
    }

    public static Dictionary<string, Symbol> Clone(this Dictionary<string, Symbol> source)
    {
        return source.ToDictionary(p => p.Key, p => p.Value.Clone());
    }

    public static List<Symbol> Clone(this IEnumerable<Symbol> source)
    {
        return source.Select(s => s.Clone()).ToList();
    }
}
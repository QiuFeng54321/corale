namespace PseudoCode.Core.CodeGen;

public class NameGenerator
{
    private readonly Dictionary<string, int> _counter = new();

    private static string Name(string str, int i)
    {
        return $"{str}.{i}";
    }

    public string Request(string str)
    {
        if (!_counter.ContainsKey(str)) _counter.Add(str, 0);

        return Name(str, _counter[str]++);
    }

    public string RequestTemp(string str)
    {
        return Request($"{ReservedNames.Temp}{str}");
    }
}
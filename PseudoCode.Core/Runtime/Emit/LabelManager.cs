namespace PseudoCode.Core.Runtime.Emit;

public class LabelManager
{
    public readonly Dictionary<string, int> PrefixCounts = new();

    public Label MakeLabel(string prefix)
    {
        if (!PrefixCounts.ContainsKey(prefix))
            PrefixCounts.Add(prefix, 0);
        return new Label(prefix, PrefixCounts[prefix]++);
    }
}
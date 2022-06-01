namespace PseudoCode.Core.Runtime;

public class Definition
{
    public Type Type;
    public string Name;
    public SourceRange SourceRange;
    public List<SourceRange> References = new();
}
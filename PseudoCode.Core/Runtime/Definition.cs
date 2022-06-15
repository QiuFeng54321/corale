using PseudoCode.Core.Runtime.Instances;
using Type = PseudoCode.Core.Runtime.Types.Type;

namespace PseudoCode.Core.Runtime;

public class Definition
{
    public string Name;
    public List<SourceRange> References = new();
    public SourceRange SourceRange;
    public Type Type;
    public Instance DefaultInstance;
}
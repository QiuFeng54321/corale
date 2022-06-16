using PseudoCode.Core.Runtime;

namespace PseudoCode.Core.Analyzing;

public class CodeFix
{
    public string Message;
    public List<Replacement> Replacements = new();

    public class Replacement
    {
        public SourceRange SourceRange;
        public string Text;
    }
}
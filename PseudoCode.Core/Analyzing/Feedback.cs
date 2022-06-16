using PseudoCode.Core.Runtime;

namespace PseudoCode.Core.Analyzing;

public class Feedback
{
    public enum SeverityType
    {
        Error,
        Warning,
        Information,
        Hint
    }

    public string Message;

    public string ReplacementMessage
    {
        get => _replacementMessage ??= Message;
        set => _replacementMessage = value;
    }
    private string _replacementMessage;
    public SeverityType Severity;
    public SourceRange SourceRange;
    public List<Replacement> Replacements = new();

    public class Replacement
    {
        public SourceRange SourceRange;
        public string Text;
    }

    public override string ToString()
    {
        return $"{Severity}: {Message} at {SourceRange}";
    }
}
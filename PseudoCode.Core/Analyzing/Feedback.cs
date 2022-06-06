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
    public SeverityType Severity;
    public SourceRange SourceRange;

    public override string ToString()
    {
        return $"{Severity}: {Message} at {SourceRange}";
    }
}
using PseudoCode.Core.Runtime;

namespace PseudoCode.Core.Analyzing;

public class Feedback
{
    public string Message;
    public SourceRange SourceRange;
    public SeverityType Severity;
    public enum SeverityType
    {
        Error,
        Warning,
        Information,
        Hint
    }
}
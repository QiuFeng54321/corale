using PseudoCode.Core.CodeGen;

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

    public List<CodeFix> CodeFixes = new();
    public DebugInformation DebugInformation;

    public string Message;

    public SeverityType Severity;

    public override string ToString()
    {
        return $"{Severity}: {Message} at {DebugInformation}";
    }
}
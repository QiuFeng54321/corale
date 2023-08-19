using Antlr4.Runtime;
using PseudoCode.Core.Analyzing;

namespace PseudoCode.Core.Parsing;

public class PseudoCodeErrorListener : BaseErrorListener
{
    public readonly PseudoCodeCompiler PseudoCodeCompiler;

    public PseudoCodeErrorListener(PseudoCodeCompiler pseudoCodeCompiler)
    {
        PseudoCodeCompiler = pseudoCodeCompiler;
    }

    public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line,
        int charPositionInLine,
        string msg, RecognitionException e)
    {
        // base.SyntaxError(output, recognizer, offendingSymbol, line, charPositionInLine, msg, e);
        output.WriteLine($"Syntax error: {msg} {offendingSymbol} {line}:{charPositionInLine} {e}");
        PseudoCodeCompiler.Program.AnalyserFeedbacks.Add(new Feedback
        {
            Message = $"Syntax error: {msg}",
            Severity = Feedback.SeverityType.Error,
            SourceRange = SourceLocationHelper.SourceRange(offendingSymbol)
        });
    }
}
using Antlr4.Runtime;
using PseudoCode.Core.Analyzing;

namespace PseudoCode.Core.Parsing;

public class PseudoCodeErrorListener : BaseErrorListener
{
    public NewCompiler PseudoCodeCompiler;

    public PseudoCodeErrorListener(NewCompiler pseudoCodeCompiler)
    {
        PseudoCodeCompiler = pseudoCodeCompiler;
    }

    public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line,
        int charPositionInLine,
        string msg, RecognitionException e)
    {
        // base.SyntaxError(output, recognizer, offendingSymbol, line, charPositionInLine, msg, e);
        output.WriteLine($"Syntax error: {msg} {offendingSymbol} {line}:{charPositionInLine} {e}");
        PseudoCodeCompiler.Context.Analysis.Feedbacks.Add(new Feedback
        {
            Message = $"Syntax error: {msg}",
            Severity = Feedback.SeverityType.Error,
            SourceRange = SourceLocationHelper.SourceRange(offendingSymbol)
        });
    }
}
using Antlr4.Runtime;

namespace PseudoCode.Core;

public class PseudoCodeErrorListener : BaseErrorListener
{
    public PseudoCodeErrorListener()
    {
    }

    public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine,
        string msg, RecognitionException e)
    {
        // base.SyntaxError(output, recognizer, offendingSymbol, line, charPositionInLine, msg, e);
        output.WriteLine($"Syntax error: {msg} {offendingSymbol} {line}:{charPositionInLine} {e}");
    }
}
using Antlr4.Runtime;
using PseudoCode.Core.Runtime;

namespace PseudoCode.Core.Parsing;

public static class SourceLocationHelper
{
    public static SourceLocation SourceLocation(IToken token)
    {
        return token == null ? null : new SourceLocation(token.Line, token.Column);
    }

    public static SourceLocation SourceLocationEnd(IToken token)
    {
        return token == null ? null : new SourceLocation(token.Line, token.Column + token.Text.Length);
    }

    public static SourceLocation SourceLocation(ParserRuleContext context)
    {
        return SourceLocation(context.Start);
    }

    public static SourceRange SourceRange(ParserRuleContext context)
    {
        return new SourceRange(SourceLocation(context.Start), SourceLocationEnd(context.Stop));
    }

    public static SourceRange SourceRange(IToken token)
    {
        // FIXME: Temp solution of token range
        return new SourceRange(SourceLocation(token), new SourceLocation(token.Line, token.Column + token.Text.Length));
    }
}
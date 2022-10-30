using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using PseudoCode.Core.Runtime;

namespace PseudoCode.Core.Parsing;

public static class SourceLocationHelper
{
    public static SourceLocation SourceLocation(this IToken token)
    {
        return token == null ? null : new SourceLocation(token.Line, token.Column);
    }

    public static SourceLocation SourceLocationEnd(this IToken token)
    {
        return token == null ? null : new SourceLocation(token.Line, token.Column + token.Text.Length);
    }

    public static SourceLocation SourceLocation(this ParserRuleContext context)
    {
        return SourceLocation(context.Start);
    }

    public static SourceRange SourceRange(this ParserRuleContext context)
    {
        return new SourceRange(SourceLocation(context.Start), SourceLocationEnd(context.Stop));
    }

    public static SourceRange SourceRange(this IToken token)
    {
        // FIXME: Temp solution of token range
        return new SourceRange(SourceLocation(token), new SourceLocation(token.Line, token.Column + token.Text.Length));
    }

    public static SourceRange SourceRange(this ITerminalNode terminalNode)
    {
        return terminalNode.Symbol.SourceRange();
    }
}
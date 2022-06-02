using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using PseudoCode.Core.Runtime;
using Range = OmniSharp.Extensions.LanguageServer.Protocol.Models.Range;

namespace PseudoCode.LSP;

public static class LocationExtension
{
    public static SourceLocation ToLocation(this Position position)
    {
        return new SourceLocation(position.Line + 1, position.Character);
    }
    public static Position ToPosition(this SourceLocation position)
    {
        return new Position(position.Line - 1, position.Column);
    }
    public static Range ToRange(this SourceRange range)
    {
        return new Range(range.Start.ToPosition(), range.End.ToPosition());
    }
    public static SourceRange ToRange(this Range range)
    {
        return new SourceRange(range.Start.ToLocation(), range.End.ToLocation());
    }
}
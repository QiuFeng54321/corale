using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using PseudoCode.Core.Runtime;

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

}
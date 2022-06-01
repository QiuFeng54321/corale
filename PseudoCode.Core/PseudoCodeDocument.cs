using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace PseudoCode.Core;

public static class PseudoCodeDocument
{
    public static IParseTree GetParseTreeFromDocument(ICharStream charStream)
    {
        ITokenSource lexer = new PseudoCodeLexer(charStream);
        ITokenStream tokens = new CommonTokenStream(lexer);
        PseudoCodeParser parser = new(tokens)
        {
            BuildParseTree = true,
        };
        parser.RemoveErrorListeners();
        parser.AddErrorListener(new PseudoCodeErrorListener());
        IParseTree parseTree = parser.fileInput();
        return parseTree;
    }
}
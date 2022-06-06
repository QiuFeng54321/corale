using Antlr4.Runtime;

namespace PseudoCode.Core.Parsing;

public static class PseudoCodeDocument
{
    public static PseudoCodeParser GetParser(ICharStream charStream)
    {
        ITokenSource lexer = new PseudoCodeLexer(charStream);
        ITokenStream tokens = new CommonTokenStream(lexer);
        PseudoCodeParser parser = new(tokens)
        {
            BuildParseTree = true
        };
        return parser;
    }

    public static void AddErrorListener(PseudoCodeParser parser, PseudoCodeCompiler compiler)
    {
        parser.RemoveErrorListeners();
        parser.AddErrorListener(new PseudoCodeErrorListener(compiler));
    }
}
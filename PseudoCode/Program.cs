// See https://aka.ms/new-console-template for more information

using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using PseudoCode;

// var input = "your text to parse here";
var stream = CharStreams.fromPath("run.pseudo");
ITokenSource lexer = new PseudoCodeLexer(stream);
ITokenStream tokens = new CommonTokenStream(lexer);
PseudoCodeParser parser = new(tokens)
{
    BuildParseTree = true
};
IParseTree tree = parser.fileInput();
var interpreter = new PseudoCodeInterpreter();
ParseTreeWalker.Default.Walk(interpreter, tree);
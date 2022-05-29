// See https://aka.ms/new-console-template for more information

using System.Globalization;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using CommandLine;
using PseudoCode;

// var input = "your text to parse here";
void RunProgram(CommandLines.Options opts)
{
    Thread.CurrentThread.CurrentCulture = new CultureInfo(opts.Locale, false);
    Thread.CurrentThread.CurrentUICulture = new CultureInfo(opts.Locale, false);
    var stream = CharStreams.fromPath(opts.FilePath);
    ITokenSource lexer = new PseudoCodeLexer(stream);
    ITokenStream tokens = new CommonTokenStream(lexer);
    PseudoCodeParser parser = new(tokens)
    {
        BuildParseTree = true
    };
    IParseTree tree = parser.fileInput();
    var interpreter = new PseudoCodeInterpreter
    {
        DisplayOperations = opts.Verbose,
        Program =
        {
            AllowUndeclaredVariables = !opts.StrictVariables
        }
    };
    ParseTreeWalker.Default.Walk(interpreter, tree);
}

void HandleParseError(IEnumerable<Error> errs)
{
    //
}
CommandLine.Parser.Default.ParseArguments<CommandLines.Options>(args)
        .WithParsed(RunProgram)
        .WithNotParsed(HandleParseError);

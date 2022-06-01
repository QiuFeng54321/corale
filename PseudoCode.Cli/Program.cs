// See https://aka.ms/new-console-template for more information

using System.Globalization;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using CommandLine;
using PseudoCode.Cli;
using PseudoCode.Core;

// var input = "your text to parse here";


void RunProgram(CommandLines.Options opts)
{
    Thread.CurrentThread.CurrentCulture = new CultureInfo(opts.Locale, false);
    Thread.CurrentThread.CurrentUICulture = new CultureInfo(opts.Locale, false);
    var stream = CharStreams.fromPath(opts.FilePath);
    var tree = PseudoCodeDocument.GetParseTreeFromDocument(stream);
    var interpreter = new PseudoCodeInterpreter
    {
        Program =
        {
            DisplayOperationsAfterCompiled = opts.PrintOperations,
            DisplayOperationsAtRuntime = opts.PrintExecutingOperation,
            AllowUndeclaredVariables = !opts.StrictVariables,
            DebugRepresentation = opts.DebugRepresentation
        }
    };
    ParseTreeWalker.Default.Walk(interpreter, tree);
}

void HandleParseError(IEnumerable<Error> errs)
{
    //
    Console.Error.WriteLine(string.Join('\n', errs));
}
CommandLine.Parser.Default.ParseArguments<CommandLines.Options>(args)
        .WithParsed(RunProgram)
        .WithNotParsed(HandleParseError);

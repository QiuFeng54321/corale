// See https://aka.ms/new-console-template for more information

using System.Globalization;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using CommandLine;
using PseudoCode.Cli;
using PseudoCode.Core;
using PseudoCode.Core.Analyzing;
using PseudoCode.Core.Parsing;

// var input = "your text to parse here";


void RunProgram(CommandLines.Options opts)
{
    Thread.CurrentThread.CurrentCulture = new CultureInfo(opts.Locale, false);
    Thread.CurrentThread.CurrentUICulture = new CultureInfo(opts.Locale, false);
    var stream = CharStreams.fromPath(opts.FilePath);
    var parser = PseudoCodeDocument.GetParser(stream);
    var interpreter = new PseudoCodeCompiler
    {
        Program =
        {
            DisplayOperationsAfterCompiled = opts.PrintOperations,
            DisplayOperationsAtRuntime = opts.PrintExecutingOperation,
            AllowUndeclaredVariables = !opts.StrictVariables,
            DebugRepresentation = opts.DebugRepresentation
        }
    };
    PseudoCodeDocument.AddErrorListener(parser, interpreter);
    IParseTree parseTree = parser.fileInput();
    var program = interpreter.Compile(parseTree);
    var analysis = new Analysis();
    analysis.SetProgram(program);
    analysis.AnalyseUnusedVariables();
    program.PrintAnalyzerFeedbacks(Console.Out);
    if (program.AnalyserFeedbacks.Any(f => f.Severity == Feedback.SeverityType.Error))
    {
        Console.WriteLine("Program will not start because there's an error");
    }
    else
        program.GlobalScope.HandledOperate();
}

void RunNewProgram(CommandLines.Options opts)
{
    Thread.CurrentThread.CurrentCulture = new CultureInfo(opts.Locale, false);
    Thread.CurrentThread.CurrentUICulture = new CultureInfo(opts.Locale, false);
    var stream = CharStreams.fromPath(opts.FilePath);
    var parser = PseudoCodeDocument.GetParser(stream);
    var interpreter = new NewCompiler
    {
    };
    // PseudoCodeDocument.AddErrorListener(parser, interpreter);
    IParseTree parseTree = parser.fileInput();
    var program = interpreter.Compile(parseTree);
    Console.WriteLine(string.Join(", ", program.Opcodes.AsEnumerable()));
    // var analysis = new Analysis();
    // analysis.SetProgram(program);
    // analysis.AnalyseUnusedVariables();
    // program.PrintAnalyzerFeedbacks(Console.Out);
    // if (program.AnalyserFeedbacks.Any(f => f.Severity == Feedback.SeverityType.Error))
    // {
    //     Console.WriteLine("Program will not start because there's an error");
    // }
    // else
    //     program.GlobalScope.HandledOperate();
}

void HandleParseError(IEnumerable<Error> errs)
{
    //
    Console.Error.WriteLine(string.Join('\n', errs));
}

CommandLine.Parser.Default.ParseArguments<CommandLines.Options>(args)
    .WithParsed(RunNewProgram)
    .WithNotParsed(HandleParseError);
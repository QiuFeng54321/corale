// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Globalization;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using CommandLine;
using LLVMSharp.Interop;
using PseudoCode.Cli;
using PseudoCode.Core.CodeGen;
using PseudoCode.Core.Parsing;
using Parser = CommandLine.Parser;

// var input = "your text to parse here";


void RunProgram(CommandLines.Options opts)
{
    unsafe
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo(opts.Locale, false);
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(opts.Locale, false);
        var stream = CharStreams.fromPath(opts.FilePath);
        var parser = PseudoCodeDocument.GetParser(stream);
        var interpreter = new NewCompiler();
        PseudoCodeDocument.AddErrorListener(parser, interpreter);
        IParseTree parseTree = parser.fileInput();
        var ctx = interpreter.Compile(parseTree);
        ctx.Analysis.PrintFeedbacks();
        Console.WriteLine(ctx.CompilationUnit.MainFunction.ToString());
        var func = ctx.Module.GetNamedFunction(ReservedNames.Main);
        var res = LLVM.VerifyFunction(func, LLVMVerifierFailureAction.LLVMPrintMessageAction);
        Debug.WriteLine(res.ToString());
        ctx.Engine.RunFunction(func, Array.Empty<LLVMGenericValueRef>());
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
}


void HandleParseError(IEnumerable<Error> errs)
{
    //
    Console.Error.WriteLine(string.Join('\n', errs));
}

Parser.Default.ParseArguments<CommandLines.Options>(args)
    .WithParsed(RunProgram)
    .WithNotParsed(HandleParseError);
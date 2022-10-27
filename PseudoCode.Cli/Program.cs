// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Globalization;
using CommandLine;
using LLVMSharp.Interop;
using PseudoCode.Cli;
using PseudoCode.Core.Parsing;

// var input = "your text to parse here";


void RunProgram(CommandLines.Options opts)
{
    unsafe
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo(opts.Locale, false);
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(opts.Locale, false);
        var compiler = new PseudoCodeCompiler(opts.FilePath);
        compiler.Compile();
        var ctx = compiler.Context;
        ctx.Analysis.PrintFeedbacks();
        Console.WriteLine(ctx.MainCompilationUnit.MainFunction.ToString());
        var func = ctx.Engine.FindFunction(ctx.MainCompilationUnit.ModuleName);
        ctx.MainCompilationUnit.Module.Dump();
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
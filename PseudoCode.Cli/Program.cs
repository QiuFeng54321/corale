// See https://aka.ms/new-console-template for more information

using System.Globalization;
using CommandLine;
using LLVMSharp.Interop;
using PseudoCode.Cli;
using PseudoCode.Core.Analyzing;
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
        ctx.MainCompilationUnit.Module.PrintToFile("out.ll");
        if (ctx.Analysis.Feedbacks.Any(f => f.Severity == Feedback.SeverityType.Error))
            Console.WriteLine("Program will not start because there's an error");
        else
            ctx.Engine.RunFunction(func, Array.Empty<LLVMGenericValueRef>());
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
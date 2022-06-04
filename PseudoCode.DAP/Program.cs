// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.DebugAdapter.Server;
using Serilog;

System.Diagnostics.Debugger.Launch();
// Debugger.Launch();
// while (!Debugger.IsAttached)
// {
//     await Task.Delay(100);
// }

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.File("/Users/mac/Documents/VSCProjects/vscode-mock-debug/daplog.txt", rollingInterval: RollingInterval.Day)
    .MinimumLevel.Verbose()
    .CreateLogger();

Log.Logger.Warning("This only goes file...");
Console.WriteLine("Hello, World!");
var server = await DebugAdapterServer.From(options =>
    options
        .WithInput(Console.OpenStandardInput())
        .WithOutput(Console.OpenStandardOutput())
        .ConfigureLogging(
            x => x
                .AddSerilog(Log.Logger)
                .SetMinimumLevel(LogLevel.Debug)
        )
    ).ConfigureAwait(false);
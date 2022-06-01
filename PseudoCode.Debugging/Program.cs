using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using CommandLine;
using static System.FormattableString;

namespace PseudoCode.Debugging;

// See https://aka.ms/new-console-template for more information

internal class ProgramArgs
{
    [Option("debug", Default = false, Required = false)]
    public bool Debug { get; set; }

    [Option("nodebug", Default = false, Required = false)]
    public bool NoDebug { get; set; }

    [Option("server", Default = 0, Required = false)]
    public int ServerPort { get; set; }

    [Option("stepOnEnter", Default = false, Required = false)]
    public bool StepOnEnter { get; set; }
}

internal class Program
{
    private static void Main(string[] args)
    {
        Parser.Default.ParseArguments<ProgramArgs>(args)
            .WithParsed(RunProgram)
            .WithNotParsed(HandleParseError);
    }

    private static void RunProgram(ProgramArgs arguments)
    {
        if (arguments.Debug)
        {
            Console.WriteLine("Waiting for debugger...");
            while (true)
            {
                if (Debugger.IsAttached)
                {
                    Console.WriteLine("Debugger attached!");
                    break;
                }

                Thread.Sleep(100);
            }
        }

        if (arguments.StepOnEnter && arguments.ServerPort == 0)
        {
            Console.WriteLine("Continue-on-Enter requires server mode.");
            return;
        }

        if (arguments.ServerPort != 0)
        {
            // Server mode - listen on a network socket
            RunServer(arguments);
        }
        else
        {
            // Standard mode - run with the adapter connected to the process's stdin and stdout
            var adapter =
                new SampleDebugAdapter(Console.OpenStandardInput(), Console.OpenStandardOutput());
            adapter.Protocol.LogMessage += (sender, e) => Debug.WriteLine(e.Message);
            adapter.Run();
        }
    }

    private static void HandleParseError(IEnumerable<Error> errors)
    {
        Console.WriteLine(string.Join('\n', errors));
        // parser.WriteUsageToConsole();
        Environment.Exit(-1);
    }

    private static void RunServer(ProgramArgs args)
    {
        Console.WriteLine(Invariant($"Waiting for connections on port {args.ServerPort}..."));
        SampleDebugAdapter adapter = null;

        var listenThread = new Thread(() =>
        {
            var listener = new TcpListener(IPAddress.Parse("127.0.0.1"), args.ServerPort);
            listener.Start();

            while (true)
            {
                var clientSocket = listener.AcceptSocket();
                var clientThread = new Thread(() =>
                {
                    Console.WriteLine("Accepted connection");

                    using (Stream stream = new NetworkStream(clientSocket))
                    {
                        adapter = new SampleDebugAdapter(stream, stream);
                        adapter.Protocol.LogMessage += (sender, e) => Console.WriteLine(e.Message);
                        adapter.Protocol.DispatcherError += (sender, e) =>
                        {
                            Console.Error.WriteLine(e.Exception.Message);
                        };
                        adapter.Run();
                        adapter.Protocol.WaitForReader();

                        adapter = null;
                    }

                    Console.WriteLine("Connection closed");
                })
                {
                    Name = "DebugServer connection thread"
                };

                clientThread.Start();
            }
        });

        Thread keypressThread;
        if (args.StepOnEnter)
        {
            Console.WriteLine("Will step when ENTER is pressed.");
            keypressThread = new Thread(() =>
            {
                ConsoleKeyInfo keyInfo;

                while (true)
                {
                    keyInfo = Console.ReadKey();
                    if (keyInfo.Key == ConsoleKey.Enter && adapter != null)
                    {
                        Console.WriteLine("Forcing step");
                        adapter.ExitBreakCore(0, true);
                    }
                }
            });

            keypressThread.Name = "Keypress listener thread";
            keypressThread.Start();
        }

        listenThread.Name = "DebugServer listener thread";
        listenThread.Start();
        listenThread.Join();
    }
}
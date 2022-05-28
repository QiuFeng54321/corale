using CommandLine;

namespace PseudoCode;

public class CommandLines
{
    public class Options
    {
        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }
        [Option('S', "strict-variables", Required = false, HelpText = "Requires every variables to be declared before use / assignment.")]
        public bool StrictVariables { get; set; }
        [Value(0, HelpText = "File to run.", Required = true)]
        public string FilePath { get; set; }
    }

}
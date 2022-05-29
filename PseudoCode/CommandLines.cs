using CommandLine;

namespace PseudoCode;

public class CommandLines
{
    public class Options
    {
        [Option('v', "verbose", Required = false, HelpText = "Prints extra info")]
        public bool Verbose { get; set; }
        [Option('S', "strict-variables", Required = false, HelpText = "Requires every variables to be declared before use / assignment.")]
        public bool StrictVariables { get; set; }
        [Option('D', "debug-representation", Required = false, HelpText = "Outputs debug representation for values")]
        public bool DebugRepresentation { get; set; }
        [Option('l', "locale", Default = "en", Required = false, HelpText = "Locale of runtime")]
        public string Locale { get; set; }
        [Value(0, HelpText = "File to run.", Required = true, MetaName = "File Path")]
        public string FilePath { get; set; }
    }

}
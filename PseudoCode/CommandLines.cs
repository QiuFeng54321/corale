using CommandLine;

namespace PseudoCode;

public class CommandLines
{
    public class Options
    {
        [Option('c', "print-operations", Required = false, HelpText = "Prints compiled operations")]
        public bool PrintOperations { get; set; }
        [Option('C', "print-executing-operations", Required = false, HelpText = "Prints operation being executed currently")]
        public bool PrintExecutingOperation { get; set; }
        [Option('S', "strict-variables", Required = false, HelpText = "Requires every variable to be declared before use / assignment.")]
        public bool StrictVariables { get; set; }
        [Option('D', "debug-representation", Required = false, HelpText = "Outputs debug representation for values")]
        public bool DebugRepresentation { get; set; }
        [Option('l', "locale", Default = "en", Required = false, HelpText = "Locale of runtime")]
        public string Locale { get; set; }
        [Value(0, HelpText = "File to run.", Required = true, MetaName = "File Path")]
        public string FilePath { get; set; }
    }

}
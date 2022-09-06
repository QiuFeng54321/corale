using Antlr4.Runtime;
using PseudoCode.Core.Parsing;

namespace PseudoCode.Core.Analyzing;

public class Analysis
{
    public readonly List<Feedback> Feedbacks = new();

    public void TolerantAnalyse(string source)
    {
        var stream = CharStreams.fromString(source);
        var parser = PseudoCodeDocument.GetParser(stream);
        var interpreter = new NewCompiler();
        PseudoCodeDocument.AddErrorListener(parser, interpreter);
        // Program = interpreter.TolerantAnalyse(parser.fileInput());
    }

    public void AnalyseUnusedVariables()
    {
    }
}
using Antlr4.Runtime;
using PseudoCode.Core.Analyzing;
using PseudoCode.Core.Parsing;
using PseudoCode.Core.Runtime;

namespace PseudoCode.LSP;

public class Analysis
{
    public PseudoProgram Program;

    public void Analyse(string source)
    {
        var stream = CharStreams.fromString(source);
        var tree = PseudoCodeDocument.GetParseTreeFromDocument(stream);
        var interpreter = new PseudoCodeCompiler();
        Program = interpreter.TolerantAnalyse(tree);
    }
}
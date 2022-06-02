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
        var parser = PseudoCodeDocument.GetParser(stream);
        var interpreter = new PseudoCodeCompiler();
        PseudoCodeDocument.AddErrorListener(parser, interpreter);
        Program = interpreter.TolerantAnalyse(parser.fileInput());
    }
}
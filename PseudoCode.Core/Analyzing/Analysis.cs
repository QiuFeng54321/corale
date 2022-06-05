using Antlr4.Runtime;
using PseudoCode.Core.Parsing;
using PseudoCode.Core.Runtime;

namespace PseudoCode.Core.Analyzing;

public class Analysis
{
    public PseudoProgram Program;
    public IEnumerable<Definition> AllVariableDefinitions;

    public void TolerantAnalyse(string source)
    {
        var stream = CharStreams.fromString(source);
        var parser = PseudoCodeDocument.GetParser(stream);
        var interpreter = new PseudoCodeCompiler();
        PseudoCodeDocument.AddErrorListener(parser, interpreter);
        Program = interpreter.TolerantAnalyse(parser.fileInput());
        SetProgram(Program);
        AnalyseUnusedVariables();
    }

    public void SetProgram(PseudoProgram program)
    {
        Program = program;
        AllVariableDefinitions = Program.GlobalScope.GetAllDefinedVariables();
    }

    public void AnalyseUnusedVariables()
    {
        if (AllVariableDefinitions == null) return;
        foreach (var definition in AllVariableDefinitions.Where(d => d.References.Count <= 1))
        {
            Program.AnalyserFeedbacks.Add(new Feedback
            {
                Message = $"Variable {definition.Name} is not used at all",
                Severity = Feedback.SeverityType.Warning,
                SourceRange = definition.SourceRange
            });
        }
    }
}
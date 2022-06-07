using Antlr4.Runtime;
using PseudoCode.Core.Parsing;
using PseudoCode.Core.Runtime;
using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.Analyzing;

public class Analysis
{
    public IEnumerable<Definition> AllVariableDefinitions;
    public PseudoProgram Program;

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
        foreach (var definition in AllVariableDefinitions)
        {
            if (definition.Type is PlaceholderType)
            {
                Program.AnalyserFeedbacks.Add(new Feedback
                {
                    Message = $"Invalid variable {definition.Name}",
                    Severity = Feedback.SeverityType.Error,
                    SourceRange = definition.SourceRange
                });
            }

            else if (definition.References.Count <= 1 && definition.SourceRange != SourceRange.Identity)
                Program.AnalyserFeedbacks.Add(new Feedback
                {
                    Message = $"Variable {definition.Name} is not used at all",
                    Severity = Feedback.SeverityType.Warning,
                    SourceRange = definition.SourceRange
                });
        }
    }
}
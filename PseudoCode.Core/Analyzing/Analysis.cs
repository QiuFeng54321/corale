using Antlr4.Runtime;
using PseudoCode.Core.Parsing;
using PseudoCode.Core.Runtime;
using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.Analyzing;

public class Analysis
{
    public IEnumerable<Definition> AllDefinitions;
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
        AllDefinitions = Program.GlobalScope.GetAllDefinedDefinitions();
    }

    public void AnalyseUnusedVariables()
    {
        if (AllDefinitions == null) return;
        foreach (var definition in AllDefinitions.Where(definition =>
                     !definition.Attributes.HasFlag(Definition.Attribute.Type)))
        {
            if (definition.Type is PlaceholderType)
            {
                var scopeSourceLocation = Program.GlobalScope
                    .GetNearestStatementScopeBefore(definition.SourceRange.Start).FirstLocation;
                Program.AnalyserFeedbacks.Add(new Feedback
                {
                    Message = $"Invalid variable {definition.Name}",
                    Severity = Feedback.SeverityType.Error,
                    SourceRange = definition.SourceRange,
                    CodeFixes = new List<CodeFix>
                    {
                        new()
                        {
                            Message = $"Declare variable {definition.Name}",
                            Replacements = new List<CodeFix.Replacement>
                            {
                                new()
                                {
                                    SourceRange = new SourceRange(scopeSourceLocation, scopeSourceLocation),
                                    Text = $"DECLARE {definition.Name} : STRING\n"
                                }
                            }
                        }
                    }
                });
            }

            else if (definition.References.Count <= 1 && definition.SourceRange != SourceRange.Identity &&
                     !definition.Name.StartsWith("_"))
                Program.AnalyserFeedbacks.Add(new Feedback
                {
                    Message = $"Variable {definition.Name} is not used at all",
                    Severity = Feedback.SeverityType.Warning,
                    SourceRange = definition.SourceRange,
                    CodeFixes = new List<CodeFix>
                    {
                        new()
                        {
                            Message = "Add an underscore before identifier",

                            Replacements = new List<CodeFix.Replacement>
                            {
                                new()
                                {
                                    SourceRange = definition.SourceRange,
                                    Text = $"_{definition.Name}"
                                }
                            }
                        }
                    }
                });
        }
    }
}
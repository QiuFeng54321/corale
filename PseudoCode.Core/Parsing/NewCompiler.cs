using Antlr4.Runtime.Tree;
using PseudoCode.Core.Runtime.Emit;

namespace PseudoCode.Core.Parsing;

public class NewCompiler : PseudoCodeBaseListener
{
    public ProgramBuilder ProgramBuilder = new();
    
    public PseudoAssembly Compile(IParseTree tree)
    {
        ParseTreeWalker.Default.Walk(this, tree);
        return ProgramBuilder.Build();
    }

    public PseudoAssembly Test()
    {
        var mainBlock = ProgramBuilder.MakeBlock(ProgramBuilder.LabelManager.MakeLabel("Start"));
        mainBlock.
        return ProgramBuilder.Build();
    }

    // public override void ExitAtom(PseudoCodeParser.AtomContext context)
    // {
    //     base.ExitAtom(context);
    //     if (context.AtomType == "ARRAY") return; // Let array handle on its own
    //     var val = context.Value;
    //     if (context.AtomType == "DATE")
    //     {
    //         if (DateOnly.TryParseExact(context.Date().GetText(), "dd/MM/yyyy", out var date))
    //         {
    //             val = date;
    //         }
    //         else
    //         {
    //             Program.AnalyserFeedbacks.Add(new Feedback
    //             {
    //                 Message = $"{context.Date().GetText()} cannot be converted into a date",
    //                 Severity = Feedback.SeverityType.Error,
    //                 SourceRange = SourceLocationHelper.SourceRange(context)
    //             });
    //             val = DateOnly.MinValue;
    //         }
    //     }
    //
    //     CurrentScope.AddOperation(new LoadImmediateOperation(CurrentScope, Program)
    //     {
    //         Intermediate = CurrentScope.FindDefinition(context.AtomType).Type.Instance(val, CurrentScope),
    //         PoiLocation = SourceLocationHelper.SourceLocation(context),
    //         SourceRange = SourceLocationHelper.SourceRange(context)
    //     });
    // }
}
global using RealNumberType = System.Double;
using Antlr4.Runtime.Tree;
using PseudoCode.Core.CodeGen;
using PseudoCode.Core.Runtime.Errors;
using Type = PseudoCode.Core.CodeGen.Type;

namespace PseudoCode.Core.Parsing;

public class NewCompiler : PseudoCodeBaseListener
{
    public CodeGenContext Context;
    public Block CurrentBlock;

    public void Initialize()
    {
        Context = new CodeGenContext();
        CurrentBlock = Context.Root;
        CurrentBlock.Namespace.AddSymbol(new Symbol("STRING", true, Type.MakePrimitiveType("STRING", typeof(string))));
        CurrentBlock.Namespace.AddSymbol(new Symbol("INTEGER", true, Type.MakePrimitiveType("INTEGER", typeof(int))));
    }


    public void Compile(IParseTree tree)
    {
        Initialize();
        ParseTreeWalker.Default.Walk(this, tree);
        var block = Context.Root.GetBlock(Context);
        Context.Module.Dump();
        // var func = Context.Module.GetNamedFunction(ReservedNames.Main);
    }

    public Symbol GetType(PseudoCodeParser.DataTypeContext context)
    {
        return GetType(context.modularDataType());
    }

    public Symbol GetType(PseudoCodeParser.ModularDataTypeContext context)
    {
        return GetType(context.typeLookup());
    }

    public Symbol GetType(PseudoCodeParser.TypeLookupContext context)
    {
        if (!CurrentBlock.Namespace.TryGetSymbol(context.Identifier().GetText(), out var sym))
            throw new InvalidAccessError($"{context.Identifier().GetText()}");
        return sym;
    }

    public override void ExitDeclarationStatement(PseudoCodeParser.DeclarationStatementContext context)
    {
        base.ExitDeclarationStatement(context);
        foreach (var id in context.identifierList().Identifier())
        {
            var symbol = new Symbol(id.GetText(), false, GetType(context.dataType()).Type);
            CurrentBlock.Namespace.AddSymbol(symbol);
            CurrentBlock.Statements.Add(new DeclarationStatement
            {
                Symbol = symbol
            });
        }
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
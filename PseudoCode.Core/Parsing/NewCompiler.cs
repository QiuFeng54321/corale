global using RealNumberType = System.Double;
using Antlr4.Runtime.Tree;
using PseudoCode.Core.Analyzing;
using PseudoCode.Core.CodeGen;
using PseudoCode.Core.Runtime.Errors;

namespace PseudoCode.Core.Parsing;

public class NewCompiler : PseudoCodeBaseListener
{
    public CodeGenContext Context;
    public Block CurrentBlock;

    public void Initialize()
    {
        Context = new CodeGenContext();
        CurrentBlock = Context.Root;
        CurrentBlock.Namespace.AddSymbol(Symbol.MakePrimitiveType("__CSTRING", typeof(string)));
        CurrentBlock.Namespace.AddSymbol(Symbol.MakePrimitiveType("INTEGER", typeof(int)));
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

    public override void ExitArithmeticExpression(PseudoCodeParser.ArithmeticExpressionContext context)
    {
        base.ExitArithmeticExpression(context);
        if (context.IsUnary)
            if (context.Identifier() is { } id)
                Context.ExpressionStack.Push(new LoadExpr
                {
                    Name = id.GetText()
                });
    }

    public override void ExitAssignmentStatement(PseudoCodeParser.AssignmentStatementContext context)
    {
        base.ExitAssignmentStatement(context);
        CurrentBlock.Statements.Add(new AssignmentStatement
        {
            Value = Context.ExpressionStack.Pop(),
            Target = Context.ExpressionStack.Pop()
        });
    }

    public override void ExitAtom(PseudoCodeParser.AtomContext context)
    {
        base.ExitAtom(context);
        if (context.AtomType == "ARRAY") return; // Let array handle on its own
        var val = context.Value;
        if (context.AtomType == "DATE")
        {
            if (DateOnly.TryParseExact(context.Date().GetText(), "dd/MM/yyyy", out var date))
            {
                val = date;
            }
            else
            {
                Context.Analysis.Feedbacks.Add(new Feedback
                {
                    Message = $"{context.Date().GetText()} cannot be converted into a date",
                    Severity = Feedback.SeverityType.Error,
                    SourceRange = SourceLocationHelper.SourceRange(context)
                });
                val = DateOnly.MinValue;
            }
        }

        switch (context.AtomType)
        {
            case "STRING":
            {
                Context.ExpressionStack.Push(new PseudoString
                {
                    Value = (string)val
                });
                break;
            }
            case "INTEGER":
            {
                Context.ExpressionStack.Push(new PseudoInteger
                {
                    Value = (int)val
                });
                break;
            }
            case "REAL":
            {
                Context.ExpressionStack.Push(new PseudoReal
                {
                    Value = (double)val
                });
                break;
            }
        }
    }
}
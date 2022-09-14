global using RealNumberType = System.Double;
using Antlr4.Runtime.Tree;
using PseudoCode.Core.Analyzing;
using PseudoCode.Core.CodeGen;
using PseudoCode.Core.Runtime.Errors;
using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.Parsing;

public class NewCompiler : PseudoCodeBaseListener
{
    public CodeGenContext Context;
    public Block CurrentBlock;

    public void Initialize()
    {
        Context = new CodeGenContext();
        CurrentBlock = Context.Root;
        BuiltinTypes.Initialize();
        BuiltinTypes.AddBuiltinTypes(CurrentBlock);
    }


    public CodeGenContext Compile(IParseTree tree)
    {
        Initialize();
        ParseTreeWalker.Default.Walk(this, tree);
        var block = Context.Root.GetBlock(Context);
        Context.Module.Dump();
        return Context;
        // var func = Context.Module.GetNamedFunction(ReservedNames.Main);
    }

    public Symbol GetType(PseudoCodeParser.DataTypeContext context)
    {
        return GetType(context.modularDataType());
    }

    public Symbol GetType(PseudoCodeParser.ModularDataTypeContext context)
    {
        var symbol = GetType(context.typeLookup()).Symbol;
        if (context.genericUtilisation() is { } genericUtilisation)
            symbol = symbol.FillGeneric(GetGenericParameters(genericUtilisation).ToList());

        return symbol;
    }

    public IEnumerable<Symbol> GetGenericParameters(PseudoCodeParser.GenericUtilisationContext context)
    {
        return context.dataTypeList().dataType().Select(GetType);
    }

    public SymbolOrNamespace GetType(PseudoCodeParser.TypeLookupContext context)
    {
        var parentNs = CurrentBlock.Namespace;
        if (context.typeLookup() != null) parentNs = GetType(context.typeLookup()).Ns;
        if (parentNs.TryGetNamespace(context.Identifier().GetText(), out var ns)) return new SymbolOrNamespace(Ns: ns);

        if (parentNs.TryGetSymbol(context.Identifier().GetText(), out var sym)) return new SymbolOrNamespace(sym);

        throw new InvalidAccessError($"{context.Identifier().GetText()}");
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

    public override void ExitLogicExpression(PseudoCodeParser.LogicExpressionContext context)
    {
        base.ExitLogicExpression(context);
        if (context.Operator == PseudoOperator.None) return;
        Expression right = null;
        if (!context.IsUnary)
            right = Context.ExpressionStack.Pop();
        var left = Context.ExpressionStack.Pop();
        Context.ExpressionStack.Push(new BinaryExpression
        {
            Left = left,
            Operator = context.Operator,
            Right = right
        });
    }

    public override void ExitArithmeticExpression(PseudoCodeParser.ArithmeticExpressionContext context)
    {
        base.ExitArithmeticExpression(context);
        if (context.Operator != PseudoOperator.None)
        {
            Expression right = null;
            if (!context.IsUnary)
                right = Context.ExpressionStack.Pop();
            var left = Context.ExpressionStack.Pop();
            Context.ExpressionStack.Push(new BinaryExpression
            {
                Left = left,
                Operator = context.Operator,
                Right = right
            });
            return;
        }

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
            case "BOOLEAN":
            {
                Context.ExpressionStack.Push(new PseudoBoolean
                {
                    Value = (bool)val
                });
                break;
            }
        }
    }

    public record SymbolOrNamespace(Symbol Symbol = default, Namespace Ns = default);
}
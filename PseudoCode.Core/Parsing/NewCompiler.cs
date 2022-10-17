global using RealNumberType = System.Double;
using Antlr4.Runtime.Tree;
using PseudoCode.Core.Analyzing;
using PseudoCode.Core.CodeGen;
using PseudoCode.Core.CodeGen.TypeLookups;
using PseudoCode.Core.Runtime.Reflection;
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
        FunctionBinder.MakeFromType(Context, CurrentBlock, typeof(BuiltinFunctions));
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

    public DataType GetType(PseudoCodeParser.DataTypeContext context)
    {
        return new DataType(GetType(context.modularDataType()));
    }

    public ModularType GetType(PseudoCodeParser.ModularDataTypeContext context)
    {
        var typeLookup = GetType(context.typeLookup());
        IEnumerable<DataType> genericParams = null;
        if (context.genericUtilisation() is { } genericUtilisation)
            genericParams = GetGenericParameters(genericUtilisation);

        return new ModularType(typeLookup, genericParams?.ToList());
    }

    public IEnumerable<DataType> GetGenericParameters(PseudoCodeParser.GenericUtilisationContext context)
    {
        return context.dataTypeList().dataType().Select(GetType);
    }

    public TypeLookup GetType(PseudoCodeParser.TypeLookupContext context)
    {
        TypeLookup parentLookup = null;
        if (context.typeLookup() != null) parentLookup = GetType(context.typeLookup());
        return new TypeLookup(context.Identifier().GetText(), parentLookup);
    }

    public override void ExitDeclarationStatement(PseudoCodeParser.DeclarationStatementContext context)
    {
        base.ExitDeclarationStatement(context);
        foreach (var id in context.identifierList().Identifier())
        {
            CurrentBlock.Statements.Add(new DeclarationStatement
            {
                Name = id.GetText(),
                DataType = GetType(context.dataType())
            });
        }
    }

    public override void ExitLogicExpression(PseudoCodeParser.LogicExpressionContext context)
    {
        base.ExitLogicExpression(context);
        if (context.OpenParen() != null)
        {
            Context.ExpressionStack.Push(new ParenthesisExpression(Context.ExpressionStack.Pop()));
            return;
        }

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

    public override void EnterTypeDefinition(PseudoCodeParser.TypeDefinitionContext context)
    {
        base.EnterTypeDefinition(context);
        CurrentBlock = CurrentBlock.EnterBlock();
    }

    public override void ExitTypeDefinition(PseudoCodeParser.TypeDefinitionContext context)
    {
        base.ExitTypeDefinition(context);
        var name = context.Identifier().GetText();
        var genericParams = context.genericDeclaration()?.identifierList()?.Identifier().Select(s => s.GetText())
            .ToList();
        var block = CurrentBlock.Statements.OfType<DeclarationStatement>().ToList();
        CurrentBlock = CurrentBlock.ParentBlock;
        CurrentBlock.Statements.RemoveAt(CurrentBlock.Statements.Count - 1);
        if (genericParams == null)
            CurrentBlock.Statements.Add(new TypeDeclaration(name, null, block));
        else
            CurrentBlock.Namespace.AddSymbol(
                Symbol.MakeTypeDeclSymbol(new TypeDeclaration(name, new GenericDeclaration(genericParams), block)));
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
        {
            if (context.Identifier() is { } id)
                Context.ExpressionStack.Push(new LoadExpr
                {
                    Name = id.GetText()
                });
            else if (context.OpenParen() != null)
                Context.ExpressionStack.Push(new ParenthesisExpression(Context.ExpressionStack.Pop()));
        }
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
}
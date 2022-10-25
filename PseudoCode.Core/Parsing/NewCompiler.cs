global using RealNumberType = System.Double;
using Antlr4.Runtime.Tree;
using PseudoCode.Core.Analyzing;
using PseudoCode.Core.CodeGen;
using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.CodeGen.TypeLookups;
using PseudoCode.Core.Runtime.Reflection;
using PseudoCode.Core.Runtime.Reflection.Builtin;
using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.Parsing;

public class NewCompiler : PseudoCodeBaseListener
{
    public CodeGenContext Context;
    public Block CurrentBlock;

    public void Initialize(string name)
    {
        BuiltinTypes.Initialize();
        Context = new CodeGenContext();
        BuiltinTypes.InitializeReflectedTypes(Context);
        CurrentBlock = Context.CompilationUnit.MainFunction.Block;
        // Context.Builder.PositionAtEnd(CurrentBlock);
        BuiltinTypes.AddBuiltinTypes(Context.GlobalNamespace);
        FunctionBinder.MakeFromType(Context, typeof(BuiltinFunctions));
        FunctionBinder.MakeFromType(Context, typeof(Printer));
        FunctionBinder.MakeFromType(Context, typeof(Scanner));
        OutputStatement.MakeConstants();
        InputStatement.MakeConstants();
    }


    public CodeGenContext Compile(IParseTree tree, string name)
    {
        Initialize(name);
        ParseTreeWalker.Default.Walk(this, tree);
        Context.CompilationUnit.CodeGen(Context, null);
        Context.Module.Dump();
        return Context;
        // var func = Context.Module.GetNamedFunction(ReservedNames.Main);
    }

    public DataType GetType(PseudoCodeParser.DataTypeContext context)
    {
        if (context.modularDataType() != null) return new DataType(GetType(context.modularDataType()));
        if (context.Array() == null) return new DataType(GetType(context.dataType()));
        throw new NotImplementedException("Array not implemented yet");
    }

    public ModularType GetType(PseudoCodeParser.ModularDataTypeContext context)
    {
        var typeLookup = GetType(context.typeLookup());
        GenericUtilisation genericParams = null;
        if (context.genericUtilisation() is { } genericUtilisation)
            genericParams = GetGenericUtilisation(genericUtilisation);

        return new ModularType(typeLookup, genericParams);
    }

    public IEnumerable<DataType> GetGenericParameters(PseudoCodeParser.GenericUtilisationContext context)
    {
        return context.dataTypeList().dataType().Select(GetType);
    }

    public GenericUtilisation GetGenericUtilisation(PseudoCodeParser.GenericUtilisationContext ctx)
    {
        var symbols = GetGenericParameters(ctx).ToList();
        return new GenericUtilisation(symbols);
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
        CurrentBlock = CurrentBlock.EnterBlock(Context.NameGenerator.Request(ReservedNames.Type));
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
                Symbol.MakeGenericSymbol(name,
                    new TypeDeclaration(name, new GenericDeclaration(genericParams), block)));
    }

    public override void ExitIfStatement(PseudoCodeParser.IfStatementContext context)
    {
        base.ExitIfStatement(context);
        Block elseBlock = null;
        if (context.HasElse)
        {
            elseBlock = (Block)CurrentBlock.Statements[^1];
            CurrentBlock.Statements.RemoveAt(CurrentBlock.Statements.Count - 1);
        }

        var thenBlock = (Block)CurrentBlock.Statements[^1];
        CurrentBlock.Statements.RemoveAt(CurrentBlock.Statements.Count - 1);
        var condExpr = Context.ExpressionStack.Pop();
        var ifStatement = new IfStatement
        {
            Condition = condExpr,
            Then = thenBlock,
            Else = elseBlock
        };
        CurrentBlock.Statements.Add(ifStatement);
    }

    public override void EnterIndentedBlock(PseudoCodeParser.IndentedBlockContext context)
    {
        base.EnterIndentedBlock(context);
        CurrentBlock = CurrentBlock.EnterBlock(Context.NameGenerator.Request(ReservedNames.Block));
    }

    public override void ExitIndentedBlock(PseudoCodeParser.IndentedBlockContext context)
    {
        base.ExitIndentedBlock(context);
        CurrentBlock = CurrentBlock.ParentBlock;
    }

    public override void ExitCallStatement(PseudoCodeParser.CallStatementContext context)
    {
        base.ExitCallStatement(context);
        var callExpr = Context.ExpressionStack.Pop();
        CurrentBlock.Statements.Add(new CallStatement
        {
            Expression = callExpr
        });
    }

    public override void ExitReturnStatement(PseudoCodeParser.ReturnStatementContext context)
    {
        base.ExitReturnStatement(context);
        CurrentBlock.Statements.Add(new ReturnStatement
        {
            Expression = Context.ExpressionStack.Pop()
        });
    }

    public override void ExitIoStatement(PseudoCodeParser.IoStatementContext context)
    {
        base.ExitIoStatement(context);
        if (context.IoKeyword().GetText() == "OUTPUT")
        {
            var exprCount = context.tuple().expression().Length;
            var exprs = new List<Expression>();
            for (var i = 0; i < exprCount; i++) exprs.Add(Context.ExpressionStack.Pop());

            exprs.Reverse();
            CurrentBlock.Statements.Add(new OutputStatement(exprs));
        }
        else if (context.IoKeyword().GetText() == "INPUT")
        {
            var expr = Context.ExpressionStack.Pop();
            CurrentBlock.Statements.Add(new InputStatement(expr));
        }
    }

    public override void EnterFunctionDefinition(PseudoCodeParser.FunctionDefinitionContext context)
    {
        base.EnterFunctionDefinition(context);
    }

    public override void ExitFunctionDefinition(PseudoCodeParser.FunctionDefinitionContext context)
    {
        base.ExitFunctionDefinition(context);
        var name = context.Identifier().GetText();
        List<FunctionDeclaration.ArgumentType> arguments = new();
        if (context.argumentsDeclaration() != null)
        {
            var byRef = false;
            foreach (var argumentDeclarationContext in context.argumentsDeclaration().argumentDeclaration())
            {
                if (argumentDeclarationContext.Byref() != null) byRef = true;
                if (argumentDeclarationContext.Byval() != null) byRef = false;
                arguments.Add(new FunctionDeclaration.ArgumentType
                {
                    Name = argumentDeclarationContext.Identifier().GetText(),
                    DataType = GetType(argumentDeclarationContext.dataType()),
                    IsRef = byRef
                });
            }
        }

        var retType = GetType(context.dataType());
        var body = (Block)CurrentBlock.Statements[^1];
        CurrentBlock.Statements.RemoveAt(CurrentBlock.Statements.Count - 1);
        var genericParams = context.genericDeclaration()?.identifierList()?.Identifier().Select(s => s.GetText())
            .ToList();
        var functionDecl = new FunctionDeclaration(name, arguments, retType, body,
            genericParams != null ? new GenericDeclaration(genericParams) : null);
        if (genericParams == null)
            CurrentBlock.Statements.Add(functionDecl);
        else
            CurrentBlock.Namespace.AddSymbol(
                Symbol.MakeGenericSymbol(name, functionDecl));
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
        else
        {
            if (context.arguments() is { } arguments)
            {
                var argumentSymbols = new List<Expression>();
                var argumentExpressions = arguments.tuple()?.expression();
                if (argumentExpressions != null)
                {
                    foreach (var expr in argumentExpressions) argumentSymbols.Add(Context.ExpressionStack.Pop());

                    argumentSymbols.Reverse();
                }

                var functionExpression = Context.ExpressionStack.Pop();
                Context.ExpressionStack.Push(new CallExpression
                {
                    Function = functionExpression,
                    Arguments = argumentSymbols
                });
            }

            if (context.Dot() != null)
            {
                var memberName = context.Identifier();
                var parentExpr = Context.ExpressionStack.Pop();
                Context.ExpressionStack.Push(new MemberAccess
                {
                    Before = parentExpr,
                    MemberName = memberName.GetText()
                });
            }

            if (context.genericUtilisation() is { } genericUtilisationContext)
            {
                var expr = Context.ExpressionStack.Pop();
                Context.ExpressionStack.Push(new ExpressionGenericUtilisation
                {
                    Expression = expr,
                    GenericUtilisation = GetGenericUtilisation(genericUtilisationContext)
                });
            }
        }
    }

    public override void ExitMallocExpression(PseudoCodeParser.MallocExpressionContext context)
    {
        base.ExitMallocExpression(context);
        var sizeExpr = Context.ExpressionStack.Pop();
        var dataType = GetType(context.dataType());
        Context.ExpressionStack.Push(new MallocExpression(sizeExpr, dataType));
    }

    public override void ExitSizeOfExpression(PseudoCodeParser.SizeOfExpressionContext context)
    {
        base.ExitSizeOfExpression(context);
        var dataType = GetType(context.dataType());
        Context.ExpressionStack.Push(new SizeOfExpression(dataType));
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
            case "CHAR":
            {
                Context.ExpressionStack.Push(new PseudoCharacter
                {
                    Value = (char)val
                });
                break;
            }
            case "INTEGER":
            {
                Context.ExpressionStack.Push(new PseudoInteger
                {
                    Value = (long)val
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
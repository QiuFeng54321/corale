global using RealNumberType = System.Double;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using PseudoCode.Core.Analyzing;
using PseudoCode.Core.CodeGen;
using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.CodeGen.TypeLookups;
using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.Parsing;

public class PseudoFileCompiler : PseudoCodeBaseListener
{
    public CompilationUnit CompilationUnit;
    public Block CurrentBlock;
    public PseudoCodeCompiler PseudoCodeCompiler;
    public CodeGenContext Context => PseudoCodeCompiler.Context;

    public void Initialize()
    {
        CurrentBlock = CompilationUnit.MainFunction.Block;
    }

    public void Compile()
    {
        var stream = CharStreams.fromPath(CompilationUnit.FilePath);
        var parser = PseudoCodeDocument.GetParser(stream);
        PseudoCodeDocument.AddErrorListener(parser, this);
        IParseTree parseTree = parser.fileInput();
        Compile(parseTree);
    }

    public CodeGenContext Compile(IParseTree tree)
    {
        Initialize();
        ParseTreeWalker.Default.Walk(this, tree);
        CompilationUnit.CodeGen(Context, CompilationUnit, null);
        Console.WriteLine("-------------");
        // CompilationUnit.Module.Dump();
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

    public NamespaceLookup GetNamespaceLookup(PseudoCodeParser.IdentiferAccessContext context)
    {
        NamespaceLookup parent = null;
        if (context.identiferAccess() is { } parentCtx) parent = GetNamespaceLookup(parentCtx);
        return new NamespaceLookup(context.Identifier().GetText(), parent);
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

    private void MakeFunction(FunctionKind functionKind, string name,
        PseudoCodeParser.GenericDeclarationContext genericDeclarationContext,
        PseudoCodeParser.ArgumentsDeclarationContext argumentsDeclarationContext, DataType returnType,
        bool isReturnByref, PseudoCodeParser.IndentedBlockContext indentedBlockContext)
    {
        List<FunctionDeclaration.ArgumentOrReturnType> arguments = new();
        var op = PseudoOperator.None;
        if (functionKind is FunctionKind.Operator) op = Enum.Parse<PseudoOperator>(name);
        if (argumentsDeclarationContext != null)
        {
            var byRef = false;
            foreach (var argumentDeclarationContext in argumentsDeclarationContext.argumentDeclaration())
            {
                if (argumentDeclarationContext.Byref() != null) byRef = true;
                if (argumentDeclarationContext.Byval() != null) byRef = false;
                arguments.Add(new FunctionDeclaration.ArgumentOrReturnType
                {
                    Name = argumentDeclarationContext.Identifier().GetText(),
                    DataType = GetType(argumentDeclarationContext.dataType()),
                    IsRef = byRef
                });
            }
        }

        var body = (Block)CurrentBlock.Statements[^1];
        CurrentBlock.Statements.RemoveAt(CurrentBlock.Statements.Count - 1);
        var genericParams = genericDeclarationContext?.identifierList()?.Identifier().Select(s => s.GetText())
            .ToList();
        returnType ??= new DataType(new ModularType(new TypeLookup("VOID")));
        var funcReturnTypeSpec = new FunctionDeclaration.ArgumentOrReturnType
        {
            DataType = returnType,
            IsRef = isReturnByref
        };
        var genericDeclaration = genericParams != null ? new GenericDeclaration(genericParams) : null;
        var functionDecl = new FunctionDeclaration(name, arguments, funcReturnTypeSpec, body, genericDeclaration, op);
        if (genericParams == null)
            CurrentBlock.Statements.Add(functionDecl);
        else
            CurrentBlock.Namespace.AddSymbol(
                Symbol.MakeGenericSymbol(name, functionDecl));
    }

    public override void ExitFunctionDefinition(PseudoCodeParser.FunctionDefinitionContext context)
    {
        base.ExitFunctionDefinition(context);
        MakeFunction(context.OperatorKeyword() is { } ? FunctionKind.Operator : FunctionKind.Function,
            context.Identifier().GetText(), context.genericDeclaration(), context.argumentsDeclaration(),
            GetType(context.dataType()), context.Byref() != null, context.indentedBlock());
    }

    public override void ExitProcedureDefinition(PseudoCodeParser.ProcedureDefinitionContext context)
    {
        base.ExitProcedureDefinition(context);
        MakeFunction(FunctionKind.Procedure,
            context.identifierWithNew().GetText(), context.genericDeclaration(), context.argumentsDeclaration(),
            null, false, context.indentedBlock());
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
            if (context.identiferAccess() is { } id)
                Context.ExpressionStack.Push(new LoadExpr
                {
                    NamespaceLookup = GetNamespaceLookup(id)
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

    public override void ExitImportStatement(PseudoCodeParser.ImportStatementContext context)
    {
        base.ExitImportStatement(context);
        var expr = Context.ExpressionStack.Pop();
        if (expr is not PseudoString pseudoString) return;
        CurrentBlock.Statements.Add(new ImportStatement(pseudoString.Value));
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

    private enum FunctionKind
    {
        Procedure,
        Function,
        Operator
    }
}
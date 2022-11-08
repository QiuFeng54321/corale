global using RealNumberType = System.Double;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using PseudoCode.Core.Analyzing;
using PseudoCode.Core.CodeGen;
using PseudoCode.Core.CodeGen.CompoundStatements;
using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.CodeGen.Expressions;
using PseudoCode.Core.CodeGen.SimpleStatements;
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
        return Context;
        // var func = Context.Module.GetNamedFunction(ReservedNames.Main);
    }

    public DataType GetType(PseudoCodeParser.DataTypeContext context)
    {
        if (context.OpenParen() != null) return GetType(context.dataType());
        if (context.modularDataType() is { })
            return new DataType(GetType(context.modularDataType()))
                .DI(CompilationUnit, context.SourceRange());

        if (context.Caret() != null)
            return new DataType(GetType(context.dataType()))
                .DI(CompilationUnit, context.SourceRange());

        if (context.expression() is { } expressionsContexts)
        {
            var expressions = expressionsContexts.Select(_ => Context.ExpressionStack.Pop()).ToList();

            expressions.Reverse();
            return new DataType(GetType(context.dataType()), expressions.ToArray())
                .DI(CompilationUnit, context.SourceRange());
        }

        throw new NotImplementedException("Array not implemented yet");
    }

    public ModularType GetType(PseudoCodeParser.ModularDataTypeContext context)
    {
        var typeLookup = GetNamespaceLookup(context.identiferAccess());
        GenericUtilisation genericParams = null;
        if (context.genericUtilisation() is { } genericUtilisation)
            genericParams = GetGenericUtilisation(genericUtilisation);

        return new ModularType(typeLookup, genericParams)
            .DI(CompilationUnit, context.SourceRange());
    }

    public NamespaceLookup GetNamespaceLookup(PseudoCodeParser.IdentiferAccessContext context)
    {
        NamespaceLookup parent = null;
        if (context.identiferAccess() is { } parentCtx) parent = GetNamespaceLookup(parentCtx);

        return new NamespaceLookup(context.Identifier().GetText(), parent)
            .DI(CompilationUnit, context.SourceRange());
    }

    public IEnumerable<DataType> GetGenericParameters(PseudoCodeParser.GenericUtilisationContext context)
    {
        return context.dataTypeList().dataType().Select(GetType);
    }

    public GenericUtilisation GetGenericUtilisation(PseudoCodeParser.GenericUtilisationContext context)
    {
        var symbols = GetGenericParameters(context).ToList();
        return new GenericUtilisation(symbols).DI(CompilationUnit, context.SourceRange());
    }


    public override void ExitDeclarationStatement(PseudoCodeParser.DeclarationStatementContext context)
    {
        base.ExitDeclarationStatement(context);
        var di = new DebugInformation(CompilationUnit, context.SourceRange(), context.identifierList().SourceRange());
        foreach (var id in context.identifierList().Identifier())
        {
            CurrentBlock.Statements.Add(new DeclarationStatement
            {
                Name = id.GetText(),
                DataType = GetType(context.dataType())
            }.DI(di));
        }
    }

    public override void ExitLogicExpression(PseudoCodeParser.LogicExpressionContext context)
    {
        base.ExitLogicExpression(context);
        if (context.OpenParen() != null)
        {
            Context.ExpressionStack.Push(
                new ParenthesisExpression(Context.ExpressionStack.Pop())
                    .DI(CompilationUnit, context.SourceRange(), context.SourceRange()));
            return;
        }

        if (context.Operator == PseudoOperator.None) return;

        Expression right = null;
        if (!context.IsUnary) right = Context.ExpressionStack.Pop();

        var left = Context.ExpressionStack.Pop();
        Context.ExpressionStack.Push(new BinaryExpression
        {
            Left = left,
            Operator = context.Operator,
            Right = right
        }.DI(CompilationUnit, context.SourceRange(), context.op.SourceRange()));
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
            CurrentBlock.Statements.Add(new TypeDeclaration(name, null, block)
                .DI(CompilationUnit, context.SourceRange(),
                    context.Identifier().SourceRange()));
        else
            CurrentBlock.Statements.Add(new AddGenericSymbolStatement
            {
                Symbol = Symbol.MakeGenericSymbol(name,
                    new TypeDeclaration(name,
                            new GenericDeclaration(genericParams).DI(CompilationUnit,
                                context.genericDeclaration().SourceRange()), block)
                        .DI(CompilationUnit, context.SourceRange(),
                            context.Identifier().SourceRange()))
            });
    }

    public override void ExitIfStatement(PseudoCodeParser.IfStatementContext context)
    {
        base.ExitIfStatement(context);
        Block elseBlock = null;
        if (context.HasElse)
        {
            elseBlock = PopStatement<Block>();
        }

        var thenBlock = PopStatement<Block>();
        var condExpr = Context.ExpressionStack.Pop();
        var ifStatement = new IfStatement
        {
            Condition = condExpr,
            Then = thenBlock,
            Else = elseBlock
        };
        ifStatement.DI(CompilationUnit, context.SourceRange());
        CurrentBlock.Statements.Add(ifStatement);
    }

    public override void ExitWhileStatement(PseudoCodeParser.WhileStatementContext context)
    {
        base.ExitWhileStatement(context);
        var thenBlock = PopStatement<Block>();
        var condExpr = Context.ExpressionStack.Pop();
        var whileStmt = new WhileStatement
        {
            Condition = condExpr,
            Then = thenBlock
        };
        whileStmt.DI(CompilationUnit, context.SourceRange());
        CurrentBlock.Statements.Add(whileStmt);
    }

    public override void ExitForStatement(PseudoCodeParser.ForStatementContext context)
    {
        base.ExitForStatement(context);
        var nextExpr = Context.ExpressionStack.Pop();
        var block = PopStatement<Block>();
        Expression step = null;
        if (context.HasStep) step = Context.ExpressionStack.Pop();
        var toExpr = Context.ExpressionStack.Pop();
        var assignmentStmt = PopStatement<AssignmentStatement>();
        var forStmt = new ForStatement
        {
            Initial = assignmentStmt,
            Target = toExpr,
            Step = step,
            Block = block,
            Next = nextExpr
        };
        forStmt.DI(CompilationUnit, context.SourceRange());
        CurrentBlock.Statements.Add(forStmt);
    }

    public T PopStatement<T>() where T : Statement
    {
        var statement = (T)CurrentBlock.Statements[^1];
        CurrentBlock.Statements.RemoveAt(CurrentBlock.Statements.Count - 1);
        return statement;
    }

    public override void ExitRepeatStatement(PseudoCodeParser.RepeatStatementContext context)
    {
        base.ExitRepeatStatement(context);
        var condExpr = Context.ExpressionStack.Pop();
        var thenBlock = (Block)CurrentBlock.Statements[^1];
        CurrentBlock.Statements.RemoveAt(CurrentBlock.Statements.Count - 1);
        var repeatStatement = new RepeatStatement
        {
            Condition = condExpr,
            Then = thenBlock
        };
        repeatStatement.DI(CompilationUnit, context.SourceRange());
        CurrentBlock.Statements.Add(repeatStatement);
    }

    public override void EnterIndentedBlock(PseudoCodeParser.IndentedBlockContext context)
    {
        base.EnterIndentedBlock(context);
        CurrentBlock = CurrentBlock.EnterBlock(Context.NameGenerator.Request(ReservedNames.Block));
    }

    public override void ExitIndentedBlock(PseudoCodeParser.IndentedBlockContext context)
    {
        base.ExitIndentedBlock(context);
        CurrentBlock.DI(CompilationUnit, context.SourceRange());
        CurrentBlock = CurrentBlock.ParentBlock;
    }

    public override void ExitCallStatement(PseudoCodeParser.CallStatementContext context)
    {
        base.ExitCallStatement(context);
        var callExpr = Context.ExpressionStack.Pop();
        CurrentBlock.Statements.Add(new CallStatement
        {
            Expression = callExpr
        }.DI(CompilationUnit, context.SourceRange(), context.Call().SourceRange()));
    }

    public override void ExitReturnStatement(PseudoCodeParser.ReturnStatementContext context)
    {
        base.ExitReturnStatement(context);
        CurrentBlock.Statements.Add(new ReturnStatement
        {
            Expression = Context.ExpressionStack.Pop()
        }.DI(CompilationUnit, context.SourceRange(), context.Return().SourceRange()));
    }

    public override void ExitIoStatement(PseudoCodeParser.IoStatementContext context)
    {
        base.ExitIoStatement(context);
        var di = new DebugInformation(CompilationUnit, context.SourceRange(), context.IoKeyword().SourceRange());
        if (context.IoKeyword().GetText() == "OUTPUT")
        {
            var exprCount = context.tuple().expression().Length;
            var exprs = new List<Expression>();
            for (var i = 0; i < exprCount; i++) exprs.Add(Context.ExpressionStack.Pop());

            exprs.Reverse();
            CurrentBlock.Statements.Add(new OutputStatement(exprs).DI(di));
        }
        else if (context.IoKeyword().GetText() == "INPUT")
        {
            var expr = Context.ExpressionStack.Pop();
            CurrentBlock.Statements.Add(new InputStatement(expr).DI(di));
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
                }.DI(CompilationUnit, argumentDeclarationContext.SourceRange(),
                    argumentDeclarationContext.Identifier().SourceRange()));
            }
        }

        var body = PopStatement<Block>();
        var genericParams = genericDeclarationContext?.identifierList()?.Identifier().Select(s => s.GetText())
            .ToList();
        returnType ??= new DataType(new ModularType(new NamespaceLookup("VOID")));
        var funcReturnTypeSpec = new FunctionDeclaration.ArgumentOrReturnType
        {
            DataType = returnType,
            IsRef = isReturnByref
        }.DI(returnType.DebugInformation);
        var genericDeclaration = genericParams != null
            ? new GenericDeclaration(genericParams)
            : null;
        genericDeclaration?.DI(CompilationUnit, genericDeclarationContext.SourceRange());
        var functionDecl = new FunctionDeclaration(name, arguments, funcReturnTypeSpec, body, genericDeclaration, op);
        functionDecl.DI(CompilationUnit, indentedBlockContext.SourceRange());
        if (genericParams == null)
            CurrentBlock.Statements.Add(functionDecl);
        else
            CurrentBlock.Statements.Add(new AddGenericSymbolStatement
            {
                Symbol = Symbol.MakeGenericSymbol(name, functionDecl)
            });
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

    public override void ExitNamespaceStatement(PseudoCodeParser.NamespaceStatementContext context)
    {
        base.ExitNamespaceStatement(context);
        var nsLookup = GetNamespaceLookup(context.identiferAccess());
        Block block = null;
        if (context.indentedBlock() != null) block = PopStatement<Block>();
        CurrentBlock.Statements.Add(new NamespaceStatement
        {
            NamespaceLookup = nsLookup,
            Block = block
        }.DI(CompilationUnit, context.SourceRange(), context.Namespace().SourceRange()));
    }

    public override void ExitUseNamespaceStatement(PseudoCodeParser.UseNamespaceStatementContext context)
    {
        base.ExitUseNamespaceStatement(context);
        var nsLookup = GetNamespaceLookup(context.identiferAccess());
        CurrentBlock.Statements.Add(new UseNamespaceStatement
        {
            NamespaceLookup = nsLookup
        }.DI(CompilationUnit, context.SourceRange()));
    }

    public override void ExitArithmeticExpression(PseudoCodeParser.ArithmeticExpressionContext context)
    {
        base.ExitArithmeticExpression(context);
        if (context.Operator != PseudoOperator.None)
        {
            Expression right = null;
            if (!context.IsUnary) right = Context.ExpressionStack.Pop();

            var left = Context.ExpressionStack.Pop();
            Context.ExpressionStack.Push(new BinaryExpression
            {
                Left = left,
                Operator = context.Operator,
                Right = right
            }.DI(CompilationUnit, context.SourceRange(), context.op?.SourceRange()));
            return;
        }

        if (context.IsUnary)
        {
            if (context.identiferAccess() is { } id)
                Context.ExpressionStack.Push(new LoadExpr
                {
                    NamespaceLookup = GetNamespaceLookup(id)
                }.DI(CompilationUnit, id.SourceRange()));
            else if (context.OpenParen() != null)
                Context.ExpressionStack.Push(new ParenthesisExpression(Context.ExpressionStack.Pop())
                    .DI(CompilationUnit, context.SourceRange()));
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
                }.DI(CompilationUnit, context.SourceRange(),
                    arguments.OpenParen().SourceRange()));
            }

            if (context.Dot() != null)
            {
                var memberName = context.Identifier();
                var parentExpr = Context.ExpressionStack.Pop();
                Context.ExpressionStack.Push(new MemberAccess
                {
                    Before = parentExpr,
                    MemberName = memberName.GetText()
                }.DI(CompilationUnit, context.SourceRange(), context.Dot().SourceRange()));
            }

            if (context.genericUtilisation() is { } genericUtilisationContext)
            {
                var expr = Context.ExpressionStack.Pop();
                Context.ExpressionStack.Push(new ExpressionGenericUtilisation
                {
                    Expression = expr,
                    GenericUtilisation = GetGenericUtilisation(genericUtilisationContext)
                }.DI(CompilationUnit, context.SourceRange(), genericUtilisationContext.SourceRange()));
            }
        }
    }

    public override void ExitMallocExpression(PseudoCodeParser.MallocExpressionContext context)
    {
        base.ExitMallocExpression(context);
        var sizeExpr = Context.ExpressionStack.Pop();
        var dataType = GetType(context.dataType());
        Context.ExpressionStack.Push(new MallocExpression(sizeExpr, dataType)
            .DI(CompilationUnit, context.SourceRange()));
    }

    public override void ExitSizeOfExpression(PseudoCodeParser.SizeOfExpressionContext context)
    {
        base.ExitSizeOfExpression(context);
        var dataType = GetType(context.dataType());
        Context.ExpressionStack.Push(new SizeOfExpression(dataType)
            .DI(CompilationUnit, context.SourceRange()));
    }

    public override void ExitAssignmentStatement(PseudoCodeParser.AssignmentStatementContext context)
    {
        base.ExitAssignmentStatement(context);
        CurrentBlock.Statements.Add(new AssignmentStatement
        {
            Value = Context.ExpressionStack.Pop(),
            Target = Context.ExpressionStack.Pop()
        }.DI(CompilationUnit, context.SourceRange(), context.AssignmentNotation().SourceRange()));
    }

    public override void ExitImportStatement(PseudoCodeParser.ImportStatementContext context)
    {
        base.ExitImportStatement(context);
        var expr = Context.ExpressionStack.Pop();
        if (expr is not PseudoString pseudoString) return;

        CurrentBlock.Statements.Add(new ImportStatement(pseudoString.Value)
            .DI(CompilationUnit, context.SourceRange()));
    }

    public override void ExitAtom(PseudoCodeParser.AtomContext context)
    {
        base.ExitAtom(context);
        if (context.AtomType == "ARRAY") return; // Let array handle on its own

        var val = context.Value;
        var di = new DebugInformation(CompilationUnit, context.SourceRange());
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
                    DebugInformation = di
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
                }.DI(di));
                break;
            }
            case "CHAR":
            {
                Context.ExpressionStack.Push(new PseudoCharacter
                {
                    Value = (char)val
                }.DI(di));
                break;
            }
            case "INTEGER":
            {
                Context.ExpressionStack.Push(new PseudoInteger
                {
                    Value = (long)val
                }.DI(di));
                break;
            }
            case "REAL":
            {
                Context.ExpressionStack.Push(new PseudoReal
                {
                    Value = (double)val
                }.DI(di));
                break;
            }
            case "BOOLEAN":
            {
                Context.ExpressionStack.Push(new PseudoBoolean
                {
                    Value = (bool)val
                }.DI(di));
                break;
            }
        }
    }

    public override void ExitArray(PseudoCodeParser.ArrayContext context)
    {
        base.ExitArray(context);
        var exprs = context.expression().Select(_ => Context.ExpressionStack.Pop()).Reverse().ToList();
        Context.ExpressionStack.Push(new ArrayExpression(exprs));
    }

    private enum FunctionKind
    {
        Procedure,
        Function,
        Operator
    }
}
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using PseudoCode.Core.Analyzing;
using PseudoCode.Core.Runtime;
using PseudoCode.Core.Runtime.Operations;
using PseudoCode.Core.Runtime.Types;
using Type = PseudoCode.Core.Runtime.Types.Type;

namespace PseudoCode.Core.Parsing;

public class PseudoCodeCompiler : PseudoCodeBaseListener
{
    public Scope CurrentScope;
    public PseudoProgram Program = new();

    public PseudoProgram Compile(IParseTree tree)
    {
        ParseTreeWalker.Default.Walk(this, tree);
        Program.GlobalScope.MetaOperate();
        return Program;
    }

    public PseudoProgram TolerantAnalyse(IParseTree tree)
    {
        try
        {
            ParseTreeWalker.Default.Walk(this, tree);
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
        }

        try
        {
            Program.GlobalScope.MetaOperate();
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
        }

        return Program;
    }

    public override void EnterFileInput(PseudoCodeParser.FileInputContext context)
    {
        base.EnterFileInput(context);
        CurrentScope = Program.GlobalScope;
        CurrentScope.PoiLocation = SourceLocationHelper.SourceLocation(context);
        CurrentScope.SourceRange = SourceLocationHelper.SourceRange(context);
    }

    public override void ExitFileInput(PseudoCodeParser.FileInputContext context)
    {
        base.ExitFileInput(context);
        if (Program.DisplayOperationsAfterCompiled)
        {
            Console.WriteLine(Program.GlobalScope);
            Console.WriteLine(strings.PseudoCodeInterpreter_ExitFileInput_OperationsStart);
        }
    }

    public void EnterScope(ParserRuleContext context)
    {
        CurrentScope = CurrentScope.AddScope(SourceLocationHelper.SourceLocation(context),
            SourceLocationHelper.SourceRange(context));
    }

    public void ExitScope(ParserRuleContext context)
    {
        CurrentScope.ParentScope.AddOperation(CurrentScope);
        CurrentScope = CurrentScope.ParentScope;
    }

    public Type GetType(PseudoCodeParser.DataTypeContext context)
    {
        var type = context.TypeName;
        var dimensions = context.arrayRange().Length;
        var resType = CurrentScope.FindTypeDefinition(type).Type;
        if (dimensions != 0)
        {
            resType = new ArrayType(CurrentScope, Program)
            {
                DimensionCount = dimensions,
                ElementType = resType
            };
        }

        return resType;
    }

    public override void ExitDeclarationStatement(PseudoCodeParser.DeclarationStatementContext context)
    {
        base.ExitDeclarationStatement(context);
        // Console.WriteLine($"DECLARE {context.IDENTIFIER().GetText()} : {context.dataType().GetText()}");
        var name = context.Identifier().GetText();
        var sourceLocation = SourceLocationHelper.SourceLocation(context);
        var resType = GetType(context.dataType());

        var sourceRange = SourceLocationHelper.SourceRange(context.Identifier().Symbol);
        var range = SourceLocationHelper.SourceRange(context);
        CurrentScope.AddOperation(new DeclareOperation(CurrentScope, Program)
        {
            Name = name,
            DimensionCount = resType is ArrayType arrayType ? arrayType.DimensionCount : 0,
            PoiLocation = sourceLocation,
            SourceRange = range,
            Definition = new Definition
            {
                Type = resType,
                Name = name,
                SourceRange = sourceRange,
                References = new List<SourceRange>
                {
                    SourceLocationHelper.SourceRange(context.Identifier().Symbol)
                }
            }
        });
    }

    public FunctionType.ParameterInfo[] GetArgumentDeclarations(PseudoCodeParser.ArgumentsDeclarationContext context)
    {
        return context.argumentDeclaration().Select(declarationContext => new FunctionType.ParameterInfo
        {
            Name = declarationContext.Identifier().GetText(),
            IsReference = declarationContext.Byref() != null,
            Definition = new Definition
            {
                Type = GetType(declarationContext.dataType()),
                Name = declarationContext.Identifier().GetText(),
                SourceRange = SourceLocationHelper.SourceRange(declarationContext.Identifier().Symbol),
                References = new List<SourceRange>
                {
                    SourceLocationHelper.SourceRange(declarationContext.Identifier().Symbol)
                }
            }
        }).ToArray();
    }

    public override void ExitFunctionDefinition(PseudoCodeParser.FunctionDefinitionContext context)
    {
        base.ExitFunctionDefinition(context);
        var sourceRange = SourceLocationHelper.SourceRange(context);
        var paramInfo = GetArgumentDeclarations(context.argumentsDeclaration());
        var returnType = GetType(context.dataType());
        var name = context.Identifier().GetText();
        var body = (Scope)CurrentScope.TakeLast();
        CurrentScope.AddOperation(new MakeFunctionOperation(CurrentScope, Program)
        {
            Name = name,
            FunctionBody = body,
            Definition = new Definition
            {
                Name = name,
                SourceRange = SourceLocationHelper.SourceRange(context.Identifier().Symbol),
                Type = new FunctionType(CurrentScope, Program)
                {
                    ReturnType = returnType,
                    ParameterInfos = paramInfo
                },
                References = new List<SourceRange>
                {
                    SourceLocationHelper.SourceRange(context.Identifier().Symbol)
                }
            },
            PoiLocation = SourceLocationHelper.SourceLocation(context.Function().Symbol),
            SourceRange = sourceRange
        });
    }

    public override void ExitProcedureDefinition(PseudoCodeParser.ProcedureDefinitionContext context)
    {
        base.ExitProcedureDefinition(context);
        var sourceRange = SourceLocationHelper.SourceRange(context);
        var paramInfo = GetArgumentDeclarations(context.argumentsDeclaration());
        var name = context.identifierWithNew().GetText(); // TODO: When class is introduced, handle NEW
        var body = (Scope)CurrentScope.TakeLast();
        CurrentScope.AddOperation(new MakeFunctionOperation(CurrentScope, Program)
        {
            Name = name,
            FunctionBody = body,
            Definition = new Definition
            {
                Name = name,
                SourceRange = SourceLocationHelper.SourceRange(context.identifierWithNew()),
                Type = new FunctionType(CurrentScope, Program)
                {
                    ReturnType = null,
                    ParameterInfos = paramInfo
                },
                References = new List<SourceRange>
                {
                    SourceLocationHelper.SourceRange(context.identifierWithNew())
                }
            },
            PoiLocation = SourceLocationHelper.SourceLocation(context.Procedure().Symbol),
            SourceRange = sourceRange
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
                Program.AnalyserFeedbacks.Add(new Feedback
                {
                    Message = $"{context.Date().GetText()} cannot be converted into a date",
                    Severity = Feedback.SeverityType.Error,
                    SourceRange = SourceLocationHelper.SourceRange(context)
                });
                val = DateOnly.MinValue;
            }
        }

        CurrentScope.AddOperation(new LoadImmediateOperation(CurrentScope, Program)
        {
            Intermediate = CurrentScope.FindTypeDefinition(context.AtomType).Type.Instance(val, CurrentScope),
            PoiLocation = SourceLocationHelper.SourceLocation(context),
            SourceRange = SourceLocationHelper.SourceRange(context)
        });
    }

    public override void ExitArray(PseudoCodeParser.ArrayContext context)
    {
        base.ExitArray(context);
        var length = context.expression().Length;
        CurrentScope.AddOperation(new FormImmediateArrayOperation(CurrentScope, Program)
        {
            Length = length,
            PoiLocation = SourceLocationHelper.SourceLocation(context),
            SourceRange = SourceLocationHelper.SourceRange(context)
        });
    }

    public override void ExitReturnStatement(PseudoCodeParser.ReturnStatementContext context)
    {
        base.ExitReturnStatement(context);
        CurrentScope.AddOperation(new ReturnOperation(CurrentScope, Program)
        {
            PoiLocation = SourceLocationHelper.SourceLocation(context.Return().Symbol),
            SourceRange = SourceLocationHelper.SourceRange(context)
        });
    }

    public override void ExitAssignmentStatement(PseudoCodeParser.AssignmentStatementContext context)
    {
        base.ExitAssignmentStatement(context);
        // Console.WriteLine($"{context.lvalue().GetText()} <- {context.expr().GetText()}");
        var sourceLocation = SourceLocationHelper.SourceLocation(context.AssignmentNotation()?.Symbol);
        CurrentScope.AddOperation(new AssignmentOperation(CurrentScope, Program)
        {
            PoiLocation = sourceLocation,
            SourceRange = SourceLocationHelper.SourceRange(context)
        });
    }

    public override void ExitArithmeticExpression(PseudoCodeParser.ArithmeticExpressionContext context)
    {
        base.ExitArithmeticExpression(context);

        if (context.array() != null)
        {
            var sourceLocation = SourceLocationHelper.SourceLocation(context.array());
            CurrentScope.AddOperation(new ArrayIndexOperation(CurrentScope, Program)
            {
                PoiLocation = sourceLocation,
                SourceRange = SourceLocationHelper.SourceRange(context),
                IndexLength = context.array().expression().Length
            });
        }
        else if (context.arguments() != null)
        {
            MakeCall(context, context.arguments());
        }
        else if (context.Identifier() != null && context.IsUnary)
        {
            var variableName = context.Identifier().GetText();
            var sourceLocation = SourceLocationHelper.SourceLocation(context);
            CurrentScope.AddOperation(new LoadOperation(CurrentScope, Program)
            {
                LoadName = variableName,
                PoiLocation = sourceLocation,
                SourceRange = SourceLocationHelper.SourceRange(context.Identifier().Symbol)
            });
        }

        if (context.op != null)
        {
            var sourceLocation = SourceLocationHelper.SourceLocation(context.op);
            var operatorMethod = context.op.Type;
            if (context.IsUnary)
            {
                // TODO ambiguous operator with Caret
                CurrentScope.AddOperation(new UnaryOperation(CurrentScope, Program)
                {
                    OperatorMethod = operatorMethod,
                    PoiLocation = sourceLocation,
                    SourceRange = SourceLocationHelper.SourceRange(context)
                });
            }
            else
            {
                CurrentScope.AddOperation(
                    new BinaryOperation(CurrentScope, Program)
                    {
                        OperatorMethod = operatorMethod,
                        PoiLocation = sourceLocation,
                        SourceRange = SourceLocationHelper.SourceRange(context)
                    });
            }
        }
    }

    private void MakeCall(ParserRuleContext context, PseudoCodeParser.ArgumentsContext argContext)
    {
        var argumentCount = argContext.tuple()?.expression()?.Length ?? 0;
        var sourceLocation = SourceLocationHelper.SourceLocation(argContext.OpenParen().Symbol);
        CurrentScope.AddOperation(new CallOperation(CurrentScope, Program)
        {
            PoiLocation = sourceLocation,
            SourceRange = SourceLocationHelper.SourceRange(context),
            ArgumentCount = argumentCount
        });
    }

    public override void ExitCallStatement(PseudoCodeParser.CallStatementContext context)
    {
        base.ExitCallStatement(context);
        var lastOperation = CurrentScope.ScopeStates.Operations.Last();
        if (lastOperation is not CallOperation)
        {
            Program.AnalyserFeedbacks.Add(new Feedback
            {
                Message = "A call statement must be followed by a valid procedure call!",
                Severity = Feedback.SeverityType.Error,
                SourceRange = SourceLocationHelper.SourceRange(context)
            });
        }
    }

    public override void ExitLogicExpression(PseudoCodeParser.LogicExpressionContext context)
    {
        base.ExitLogicExpression(context);
        var op = context.op ?? context.comp?.Start;
        if (op != null)
            if (context.IsUnary)
                CurrentScope.AddOperation(new UnaryOperation(CurrentScope, Program)
                {
                    OperatorMethod = op.Type,
                    PoiLocation = SourceLocationHelper.SourceLocation(context.op),
                    SourceRange = SourceLocationHelper.SourceRange(context)
                });
            else
                CurrentScope.AddOperation(new BinaryOperation(CurrentScope, Program)
                {
                    OperatorMethod = op.Type,
                    PoiLocation = SourceLocationHelper.SourceLocation(context.op),
                    SourceRange = SourceLocationHelper.SourceRange(context)
                });
    }

    public override void ExitFileStatement(PseudoCodeParser.FileStatementContext context)
    {
        base.ExitFileStatement(context);
        var sourceRange = SourceLocationHelper.SourceRange(context);
        var sourceLocation = SourceLocationHelper.SourceLocation(context.Start);
        if (context.OpenFile() != null)
        {
            var random = false;
            var fileAccess = FileAccess.Read;
            var fileMode = FileMode.OpenOrCreate;
            if (context.Read() != null) fileAccess = FileAccess.Read;
            if (context.Append() != null) fileMode = FileMode.Append;
            if (context.Random() != null) random = true;
            CurrentScope.AddOperation(new OpenFileOperation(CurrentScope, Program)
            {
                FileAccess = fileAccess,
                FileMode = fileMode,
                IsRandom = random,
                PoiLocation = sourceLocation,
                SourceRange = sourceRange
            });
        }
        else if (context.ReadFile() != null)
        {
            CurrentScope.AddOperation(new ReadFileOperation(CurrentScope, Program)
            {
                PoiLocation = sourceLocation,
                SourceRange = sourceRange
            });
        }
    }

    public override void ExitIoStatement(PseudoCodeParser.IoStatementContext context)
    {
        base.ExitIoStatement(context);
        // Console.WriteLine($"{context.IO_KEYWORD()} {context.expression().GetText()}");
        var action = context.IoKeyword().GetText();
        switch (action)
        {
            case "OUTPUT":
                CurrentScope.AddOperation(new OutputOperation(CurrentScope, Program)
                {
                    ArgumentCount = context.tuple().expression().Length,
                    PoiLocation = SourceLocationHelper.SourceLocation(context),
                    SourceRange = SourceLocationHelper.SourceRange(context)
                });
                break;
            case "INPUT":
                CurrentScope.AddOperation(new InputOperation(CurrentScope, Program)
                {
                    PoiLocation = SourceLocationHelper.SourceLocation(context),
                    SourceRange = SourceLocationHelper.SourceRange(context)
                });
                break;
        }
    }

    public override void ExitIfStatement(PseudoCodeParser.IfStatementContext context)
    {
        base.ExitIfStatement(context);
        var falseBlock = context.HasElse ? CurrentScope.TakeLast() : null;
        var trueBlock = CurrentScope.TakeLast();
        var testScope = (Scope)CurrentScope.TakeLast();
        CurrentScope.AddOperation(new IfOperation(CurrentScope, Program)
        {
            FalseBlock = falseBlock, TrueBlock = trueBlock, TestExpressionScope = testScope,
            PoiLocation = SourceLocationHelper.SourceLocation(context),
            SourceRange = SourceLocationHelper.SourceRange(context)
        });
    }

    public override void ExitWhileStatement(PseudoCodeParser.WhileStatementContext context)
    {
        base.ExitWhileStatement(context);
        var repeatBlock = CurrentScope.TakeLast();
        var testScope = (Scope)CurrentScope.TakeLast();
        CurrentScope.AddOperation(new WhileOperation(CurrentScope, Program)
        {
            RepeatBlock = repeatBlock, TestExpressionScope = testScope,
            PoiLocation = SourceLocationHelper.SourceLocation(context),
            SourceRange = SourceLocationHelper.SourceRange(context)
        });
    }

    public override void ExitRepeatStatement(PseudoCodeParser.RepeatStatementContext context)
    {
        base.ExitRepeatStatement(context);
        var testScope = (Scope)CurrentScope.TakeLast();
        // We negate the result of test since the test is in UNTIL, not WHILE
        testScope.AddOperation(new UnaryOperation(testScope, Program)
        {
            OperatorMethod = PseudoCodeParser.Not,
            PoiLocation = SourceLocationHelper.SourceLocation(context.scopedExpression().Stop),
            SourceRange = SourceLocationHelper.SourceRange(context)
        });
        var repeatBlock = (Scope)CurrentScope.TakeLast();
        repeatBlock.Join(testScope);
        CurrentScope.AddOperation(new RepeatOperation(CurrentScope, Program)
        {
            RepeatBlock = repeatBlock,
            PoiLocation = SourceLocationHelper.SourceLocation(context),
            SourceRange = SourceLocationHelper.SourceRange(context)
        });
    }

    public override void ExitForStatement(PseudoCodeParser.ForStatementContext context)
    {
        base.ExitForStatement(context);
        var next = CurrentScope.TakeLast();
        var body = CurrentScope.TakeLast();
        var step = context.HasStep
            ? CurrentScope.TakeLast()
            : new LoadImmediateOperation(CurrentScope, Program)
            {
                Intermediate = CurrentScope.FindTypeDefinition(Type.IntegerId).Type.Instance(1, CurrentScope),
                PoiLocation = SourceLocationHelper.SourceLocation(context.Next().Symbol),
                SourceRange = SourceLocationHelper.SourceRange(context)
            };
        var target = CurrentScope.TakeLast();
        CurrentScope.AddOperation(new AssignmentOperation(CurrentScope, Program)
        {
            KeepVariableInStack = true,
            PoiLocation = SourceLocationHelper.SourceLocation(context.AssignmentNotation().Symbol)
        });
        CurrentScope.AddOperation(new ForOperation(CurrentScope, Program)
        {
            ForBody = body,
            Next = next,
            Step = step,
            TargetValue = target,
            PoiLocation = SourceLocationHelper.SourceLocation(context),
            SourceRange = SourceLocationHelper.SourceRange(context)
        });
    }

    #region ScopedExpressions

    public override void EnterIndentedBlock(PseudoCodeParser.IndentedBlockContext context)
    {
        base.EnterIndentedBlock(context);
        EnterScope(context);
    }

    public override void ExitIndentedBlock(PseudoCodeParser.IndentedBlockContext context)
    {
        base.ExitIndentedBlock(context);
        ExitScope(context);
    }

    public override void EnterAlignedBlock(PseudoCodeParser.AlignedBlockContext context)
    {
        base.EnterAlignedBlock(context);
        EnterScope(context);
    }

    public override void ExitAlignedBlock(PseudoCodeParser.AlignedBlockContext context)
    {
        base.ExitAlignedBlock(context);
        ExitScope(context);
    }

    public override void EnterScopedExpression(PseudoCodeParser.ScopedExpressionContext context)
    {
        base.EnterScopedExpression(context);
        EnterScope(context);
    }

    public override void ExitScopedExpression(PseudoCodeParser.ScopedExpressionContext context)
    {
        base.ExitScopedExpression(context);
        ExitScope(context);
    }

    #endregion
}
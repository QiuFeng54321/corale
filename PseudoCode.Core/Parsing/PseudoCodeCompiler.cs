using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using PseudoCode.Core.Analyzing;
using PseudoCode.Core.Runtime;
using PseudoCode.Core.Runtime.Operations;
using PseudoCode.Core.Runtime.Types;
using PseudoCode.Core.Runtime.Types.Descriptor;
using Type = PseudoCode.Core.Runtime.Types.Type;

namespace PseudoCode.Core.Parsing;

public class PseudoCodeCompiler : PseudoCodeBaseListener
{
    public Scope CurrentScope;
    public Type CurrentType;
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


    public override void ExitDataType(PseudoCodeParser.DataTypeContext context)
    {
        base.ExitDataType(context);
        if (context.basicDataType() != null)
        {
            context.TypeDescriptor = new PlainTypeDescriptor(context.basicDataType().GetText());
        }
        else if (context.Array() != null)
        {
            context.TypeDescriptor =
                new ArrayDescriptor(context.dataType().TypeDescriptor, context.arrayRange().Length);
        }
        else if (context.Caret() != null)
        {
            context.TypeDescriptor = new PointerDescriptor(context.dataType().TypeDescriptor);
        }
        else
        {
            Program.AnalyserFeedbacks.Add(new Feedback()
            {
                Message = $"Data type is not properly specified: {context.GetText()}",
                Severity = Feedback.SeverityType.Error,
                SourceRange = SourceLocationHelper.SourceRange(context)
            });
        }
    }

    public override void ExitDeclarationStatement(PseudoCodeParser.DeclarationStatementContext context)
    {
        base.ExitDeclarationStatement(context);
        // Console.WriteLine($"DECLARE {context.IDENTIFIER().GetText()} : {context.dataType().GetText()}");
        var name = context.Identifier().GetText();
        var sourceLocation = SourceLocationHelper.SourceLocation(context);
        var resType = context.dataType().TypeDescriptor;

        var sourceRange = SourceLocationHelper.SourceRange(context.Identifier().Symbol);
        var range = SourceLocationHelper.SourceRange(context);
        var definition = new Definition(CurrentScope, Program)
        {
            TypeDescriptor = resType,
            Name = name,
            SourceRange = sourceRange,
            References = new List<SourceRange>
            {
                SourceLocationHelper.SourceRange(context.Identifier().Symbol)
            },
            Attributes = Definition.Attribute.Reference
        };
        if (CurrentType != null)
        {
            CurrentType.Members.Add(name, definition);
        }
        else
        {
            CurrentScope.AddOperation(new DeclareOperation(CurrentScope, Program)
            {
                PoiLocation = sourceLocation,
                SourceRange = range,
                Definition = definition
            });
        }
    }

    public override void ExitConstantStatement(PseudoCodeParser.ConstantStatementContext context)
    {
        base.ExitConstantStatement(context);
        // Console.WriteLine($"DECLARE {context.IDENTIFIER().GetText()} : {context.dataType().GetText()}");
        var name = context.Identifier().GetText();
        var sourceLocation = SourceLocationHelper.SourceLocation(context);

        var sourceRange = SourceLocationHelper.SourceRange(context.Identifier().Symbol);
        var range = SourceLocationHelper.SourceRange(context);
        CurrentScope.AddOperation(new MakeConstOperation(CurrentScope, Program)
        {
            PoiLocation = sourceLocation,
            SourceRange = range,
            Definition = new Definition(CurrentScope, Program)
            {
                Name = name,
                SourceRange = sourceRange,
                References = new List<SourceRange>
                {
                    SourceLocationHelper.SourceRange(context.Identifier().Symbol)
                },
                Attributes = Definition.Attribute.Const | Definition.Attribute.Reference |
                             Definition.Attribute.Immutable
            }
        });
    }

    public Definition[] GetArgumentDeclarations(PseudoCodeParser.ArgumentsDeclarationContext context)
    {
        if (context?.argumentDeclaration() == null) return Array.Empty<Definition>();
        var passByRef = false;
        List<Definition> res = new();
        foreach (var declarationContext in context.argumentDeclaration())
        {
            if (declarationContext.Byref() != null) passByRef = true;
            if (declarationContext.Byval() != null) passByRef = false;
            res.Add(new Definition(CurrentScope, Program)
            {
                TypeDescriptor = declarationContext.dataType().TypeDescriptor,
                Name = declarationContext.Identifier().GetText(),
                SourceRange = SourceLocationHelper.SourceRange(declarationContext.Identifier().Symbol),
                References = new List<SourceRange>
                {
                    SourceLocationHelper.SourceRange(declarationContext.Identifier().Symbol)
                },
                Attributes = passByRef
                    ? Definition.Attribute.Reference
                    // : Definition.Attribute.Immutable // CAIE guide actually allows param assignment (P20 8.3 Example)
                    : Definition.Attribute.None
            });
        }

        return res.ToArray();
    }

    public override void ExitFunctionDefinition(PseudoCodeParser.FunctionDefinitionContext context)
    {
        base.ExitFunctionDefinition(context);
        var sourceRange = SourceLocationHelper.SourceRange(context);
        var paramInfo = GetArgumentDeclarations(context.argumentsDeclaration());
        var returnType = context.dataType().TypeDescriptor;
        var name = context.Identifier().GetText();
        var body = (Scope)CurrentScope.TakeLast();
        CurrentScope.AddOperation(new MakeFunctionOperation(CurrentScope, Program)
        {
            Name = name,
            FunctionBody = body,
            Definition = new Definition(CurrentScope, Program)
            {
                Name = name,
                SourceRange = SourceLocationHelper.SourceRange(context.Identifier().Symbol),
                TypeDescriptor = new FunctionDescriptor(returnType, paramInfo),
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
            Definition = new Definition(CurrentScope, Program)
            {
                Name = name,
                SourceRange = SourceLocationHelper.SourceRange(context.identifierWithNew()),
                TypeDescriptor = new FunctionDescriptor(null, paramInfo),
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
            Intermediate = CurrentScope.FindDefinition(context.AtomType).Type.Instance(val, CurrentScope),
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
        else if (context.Dot() != null)
        {
            var sourceLocation = SourceLocationHelper.SourceLocation(context.Dot().Symbol);
            CurrentScope.AddOperation(new MemberAccessOperation(CurrentScope, Program)
            {
                PoiLocation = sourceLocation,
                SourceRange = SourceLocationHelper.SourceRange(context),
                MemberName = context.Identifier().GetText()
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
            var operatorMethod = context.Operator;
            if (context.IsUnary)
                CurrentScope.AddOperation(new UnaryOperation(CurrentScope, Program)
                {
                    OperatorMethod = operatorMethod,
                    PoiLocation = sourceLocation,
                    SourceRange = SourceLocationHelper.SourceRange(context)
                });
            else
                CurrentScope.AddOperation(
                    new BinaryOperation(CurrentScope, Program)
                    {
                        OperatorMethod = operatorMethod,
                        PoiLocation = sourceLocation,
                        SourceRange = SourceLocationHelper.SourceRange(context)
                    });
        }
    }

    private CallOperation MakeCall(ParserRuleContext context, PseudoCodeParser.ArgumentsContext argContext)
    {
        var argumentCount = argContext?.tuple()?.expression()?.Length ?? 0;
        var sourceLocation = SourceLocationHelper.SourceLocation(argContext?.OpenParen()?.Symbol ?? context.Stop);
        var callOperation = new CallOperation(CurrentScope, Program)
        {
            PoiLocation = sourceLocation,
            SourceRange = SourceLocationHelper.SourceRange(context),
            ArgumentCount = argumentCount
        };
        CurrentScope.AddOperation(callOperation);
        return callOperation;
    }

    public override void ExitCallStatement(PseudoCodeParser.CallStatementContext context)
    {
        base.ExitCallStatement(context);
        var lastOperation = CurrentScope.ScopeStates.Operations.Last();
        if (lastOperation is not CallOperation)
        {
            lastOperation = MakeCall(context, null);
        }

        ((CallOperation)lastOperation).IsStatement = true;
    }

    public override void ExitLogicExpression(PseudoCodeParser.LogicExpressionContext context)
    {
        base.ExitLogicExpression(context);
        var op = context.op;
        if (op != null)
            if (context.IsUnary)
                CurrentScope.AddOperation(new UnaryOperation(CurrentScope, Program)
                {
                    OperatorMethod = context.Operator,
                    PoiLocation = SourceLocationHelper.SourceLocation(context.op),
                    SourceRange = SourceLocationHelper.SourceRange(context)
                });
            else
                CurrentScope.AddOperation(new BinaryOperation(CurrentScope, Program)
                {
                    OperatorMethod = context.Operator,
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
            fileAccess = context.Read() != null ? FileAccess.Read : FileAccess.Write;
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
        else if (context.WriteFile() != null)
        {
            CurrentScope.AddOperation(new WriteFileOperation(CurrentScope, Program)
            {
                PoiLocation = sourceLocation,
                SourceRange = sourceRange
            });
        }
        else if (context.CloseFile() != null)
        {
            CurrentScope.AddOperation(new CloseFileOperation(CurrentScope, Program)
            {
                PoiLocation = sourceLocation,
                SourceRange = sourceRange
            });
        }
        else if (context.Seek() != null)
        {
            CurrentScope.AddOperation(new SeekOperation(CurrentScope, Program)
            {
                PoiLocation = sourceLocation,
                SourceRange = sourceRange
            });
        }
        else if (context.PutRecord() != null)
        {
            CurrentScope.AddOperation(new PutRecordOperation(CurrentScope, Program)
            {
                PoiLocation = sourceLocation,
                SourceRange = sourceRange
            });
        }
        else if (context.GetRecord() != null)
        {
            CurrentScope.AddOperation(new GetRecordOperation(CurrentScope, Program)
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

    public override void ExitCaseStatement(PseudoCodeParser.CaseStatementContext context)
    {
        base.ExitCaseStatement(context);
        var list = new List<(Scope Condition, Scope Operation)>();
        Scope defaultScope = null;
        foreach (var caseBranchContext in context.caseBody().caseBranch().Reverse())
        {
            if (caseBranchContext.NL() != null) continue;
            Scope condition = null;
            var body = (Scope)CurrentScope.TakeLast();
            if (caseBranchContext.Otherwise() != null)
            {
                defaultScope = body;
                continue;
            }

            if (caseBranchContext.scopedExpression() != null)
            {
                condition = (Scope)CurrentScope.TakeLast();
                condition.InsertOperation(0, new DuplicateOperation(condition, Program)
                {
                    PoiLocation = condition.PoiLocation,
                    SourceRange = condition.SourceRange
                });
                condition.AddOperation(new BinaryOperation(condition.ParentScope, Program)
                {
                    OperatorMethod = PseudoOperator.Equal,
                    PoiLocation = condition.PoiLocation,
                    SourceRange = condition.SourceRange
                });
            }
            else if (caseBranchContext.valueRange() != null)
            {
                var to = (Scope)CurrentScope.TakeLast();
                condition = (Scope)CurrentScope.TakeLast();
                var poiLocation = SourceLocationHelper.SourceLocation(caseBranchContext.valueRange().To().Symbol);
                var sourceRange = SourceLocationHelper.SourceRange(caseBranchContext.valueRange());
                // [t, t, from]
                condition.InsertOperation(0, new DuplicateOperation(condition, Program)
                {
                    PoiLocation = poiLocation,
                    SourceRange = sourceRange
                });
                // [t, t, func, from]
                condition.InsertOperation(1, new LoadOperation(CurrentScope, Program)
                {
                    LoadName = "__in_range",
                    PoiLocation = poiLocation,
                    SourceRange = sourceRange
                });
                // [t, func, t, from]
                condition.InsertOperation(2, new SwapOperation(CurrentScope, Program)
                {
                    PoiLocation = poiLocation,
                    SourceRange = sourceRange
                });
                // [t, func, t, from, to]
                condition.Join(to);
                condition.AddOperation(new CallOperation(CurrentScope, Program)
                {
                    ArgumentCount = 3,
                    PoiLocation = poiLocation,
                    SourceRange = sourceRange
                });
            }

            list.Insert(0, (condition, body));
        }

        CurrentScope.AddOperation(new CaseOperation(CurrentScope, Program)
        {
            Cases = list,
            DefaultCase = defaultScope,
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
            OperatorMethod = PseudoOperator.Not,
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
                Intermediate = CurrentScope.FindDefinition(Type.IntegerId).Type.Instance(1, CurrentScope),
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

    public override void EnterTypeDefinition(PseudoCodeParser.TypeDefinitionContext context)
    {
        base.EnterTypeDefinition(context);
        CurrentType = new TypeType(CurrentScope, Program)
        {
            Name = context.Identifier().GetText()
        };
    }

    public override void ExitTypeDefinition(PseudoCodeParser.TypeDefinitionContext context)
    {
        base.ExitTypeDefinition(context);
        var sourceRange = SourceLocationHelper.SourceRange(context);
        CurrentScope.AddTypeDefinition(CurrentType.Name, new Definition(CurrentScope, Program)
        {
            Type = CurrentType,
            Name = CurrentType.Name,
            SourceRange = sourceRange,
            References = new List<SourceRange> { sourceRange },
            Attributes = Definition.Attribute.Type
        }, sourceRange);
        CurrentType = null;
    }

    public override void EnterClassDefinition(PseudoCodeParser.ClassDefinitionContext context)
    {
        base.EnterClassDefinition(context);
        CurrentType = new ObjectType(CurrentScope, Program)
        {
            Name = context.className.Text,
            InheritTypeDef = CurrentScope.FindDefinition(context.inheritClass.Text)
        };
    }

    public override void ExitClassDefinition(PseudoCodeParser.ClassDefinitionContext context)
    {
        base.ExitClassDefinition(context);
        var sourceRange = SourceLocationHelper.SourceRange(context);
        CurrentScope.AddTypeDefinition(CurrentType.Name, new Definition(CurrentScope, Program)
        {
            Type = CurrentType,
            Name = CurrentType.Name,
            SourceRange = sourceRange,
            References = new List<SourceRange> { sourceRange },
            Attributes = Definition.Attribute.Type
        }, sourceRange);
        CurrentType = null;
    }

    public override void ExitTypeAliasDefinition(PseudoCodeParser.TypeAliasDefinitionContext context)
    {
        base.ExitTypeAliasDefinition(context);
        CurrentScope.AddOperation(new AddTypeOperation(CurrentScope, Program)
        {
            Name = context.Identifier().GetText(),
            TypeDescriptor = context.dataType().TypeDescriptor,
            PoiLocation = SourceLocationHelper.SourceLocation(context),
            SourceRange = SourceLocationHelper.SourceRange(context)
        });
    }

    public override void ExitEnumDefinition(PseudoCodeParser.EnumDefinitionContext context)
    {
        base.ExitEnumDefinition(context);

        var autoVal = -1;
        Dictionary<string, int> enumItems = new();
        Dictionary<int, string> names = new();
        foreach (var enumItemContext in context.enumBody().enumItem())
        {
            var name = enumItemContext.Identifier().GetText();
            var valueContext = enumItemContext.integer();
            var value = valueContext == null ? ++autoVal : autoVal = int.Parse(valueContext.GetText());
            if (!enumItems.TryAdd(name, value))
                Program.AnalyserFeedbacks.Add(new Feedback
                {
                    Message = $"Duplicate enum name {name}",
                    Severity = Feedback.SeverityType.Error,
                    SourceRange = SourceLocationHelper.SourceRange(enumItemContext)
                });

            names[value] = name;
        }

        CurrentScope.AddOperation(new MakeEnumOperation(CurrentScope, Program)
        {
            Name = context.name.Text,
            Values = enumItems,
            Names = names,
            PoiLocation = SourceLocationHelper.SourceLocation(context),
            SourceRange = SourceLocationHelper.SourceRange(context)
        });
    }

    #region ScopedExpressions

    public override void EnterIndentedBlock(PseudoCodeParser.IndentedBlockContext context)
    {
        base.EnterIndentedBlock(context);
        EnterScope(context);
        CurrentScope.AllowStatements = true;
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
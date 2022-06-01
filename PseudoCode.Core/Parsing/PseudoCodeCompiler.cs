using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using PseudoCode.Core.Runtime;
using PseudoCode.Core.Runtime.Operations;
using Type = PseudoCode.Core.Runtime.Type;

namespace PseudoCode.Core.Parsing;

public class PseudoCodeCompiler : PseudoCodeBaseListener
{
    public Scope CurrentScope;
    public PseudoProgram Program = new();

    public PseudoProgram Compile(IParseTree tree)
    {
        ParseTreeWalker.Default.Walk(this, tree);
        return Program;
    }

    public override void EnterFileInput(PseudoCodeParser.FileInputContext context)
    {
        base.EnterFileInput(context);
        CurrentScope = Program.GlobalScope;
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

    public void EnterScope(SourceLocation sourceLocation = default)
    {
        CurrentScope = CurrentScope.AddScope(sourceLocation);
    }

    public void ExitScope(SourceLocation sourceLocation = default)
    {
        CurrentScope.ParentScope.AddOperation(CurrentScope);
        CurrentScope.SourceRange.End = sourceLocation;
        CurrentScope = CurrentScope.ParentScope;
    }

    public override void ExitDeclarationStatement(PseudoCodeParser.DeclarationStatementContext context)
    {
        base.ExitDeclarationStatement(context);
        // Console.WriteLine($"DECLARE {context.IDENTIFIER().GetText()} : {context.dataType().GetText()}");
        var name = context.Identifier().GetText();
        var type = context.dataType().TypeName;
        var dimensions = context.dataType().Dimensions;
        var sourceLocation = SourceLocation(context);
        CurrentScope.AddOperation(new DeclareOperation(CurrentScope, Program)
        {
            Name = name,
            TypeName = type,
            Dimensions = dimensions,
            PoiLocation = sourceLocation
        });
        var staticType = CurrentScope.TypeTable.FindType(type);
        CurrentScope.TypeTable.VariableInfos.Add(name,
            new TypeTable.VariableInfo
            {
                Type = dimensions.Count == 0
                    ? staticType
                    : new TypeTable.ArrayTypeInfo
                    {
                        DeclarationLocation = sourceLocation,
                        Dimensions = (uint)dimensions.Count,
                        ElementTypeInfo = staticType
                    },
                DeclarationLocation = sourceLocation,
                Name = name
            }
        );
    }

    private static SourceLocation SourceLocation(IToken token)
    {
        return token == null ? null : new SourceLocation(token.Line, token.Column);
    }

    private static SourceLocation SourceLocation(ParserRuleContext context)
    {
        return SourceLocation(context.Start);
    }

    private static SourceRange SourceRange(ParserRuleContext context)
    {
        return new SourceRange(SourceLocation(context.Start), SourceLocation(context.Stop));
    }

    public override void ExitAtom(PseudoCodeParser.AtomContext context)
    {
        base.ExitAtom(context);
        if (context.AtomType == "ARRAY") return; // Let array handle on its own
        CurrentScope.AddOperation(new LoadImmediateOperation(CurrentScope, Program)
        {
            Intermediate = CurrentScope.FindType(context.AtomType).Instance(context.Value, CurrentScope),
            PoiLocation = SourceLocation(context),
            SourceRange = SourceRange(context)
        });
        CurrentScope.TypeTable.AddToStack(new TypeTable.TypeInfo
        {
            DeclarationLocation = SourceLocation(context),
            Name = context.AtomType
        });
    }

    public override void ExitArray(PseudoCodeParser.ArrayContext context)
    {
        base.ExitArray(context);
        var length = context.expression().Length;
        CurrentScope.AddOperation(new FormImmediateArrayOperation(CurrentScope, Program)
        {
            Length = length,
            PoiLocation = SourceLocation(context),
            SourceRange = SourceRange(context)
        });
        CurrentScope.TypeTable.FormArray(length, SourceLocation(context));
    }

    public override void ExitAssignmentStatement(PseudoCodeParser.AssignmentStatementContext context)
    {
        base.ExitAssignmentStatement(context);
        // Console.WriteLine($"{context.lvalue().GetText()} <- {context.expr().GetText()}");
        var sourceLocation = SourceLocation(context.AssignmentNotation()?.Symbol);
        CurrentScope.AddOperation(new AssignmentOperation(CurrentScope, Program)
        {
            PoiLocation = sourceLocation,
            SourceRange = SourceRange(context)
        });
        CurrentScope.TypeTable.Assign(sourceLocation);
    }

    public override void ExitArithmeticExpression(PseudoCodeParser.ArithmeticExpressionContext context)
    {
        base.ExitArithmeticExpression(context);

        if (context.array() != null)
        {
            var sourceLocation = SourceLocation(context.array());
            CurrentScope.AddOperation(new ArrayIndexOperation(CurrentScope, Program)
            {
                PoiLocation = sourceLocation,
                SourceRange = SourceRange(context)
            });
            CurrentScope.TypeTable.ArrayAccess(sourceLocation);
        }
        else if (context.Identifier() != null && context.IsUnary)
        {
            var variableName = context.Identifier().GetText();
            var sourceLocation = SourceLocation(context);
            CurrentScope.AddOperation(new LoadOperation(CurrentScope, Program)
            {
                LoadName = variableName,
                PoiLocation = sourceLocation,
                SourceRange = SourceRange(context)
            });
            CurrentScope.TypeTable.AddToStack(CurrentScope.TypeTable.FindVariable(variableName, sourceLocation));
        }

        if (context.op != null)
        {
            var sourceLocation = SourceLocation(context.op);
            if (context.IsUnary)
            {
                // TODO ambiguous operator with Caret
                CurrentScope.AddOperation(new UnaryOperation(CurrentScope, Program)
                {
                    OperatorMethod = context.op.Type,
                    PoiLocation = sourceLocation,
                    SourceRange = SourceRange(context)
                });
                CurrentScope.TypeTable.MakeBinary(sourceLocation);
            }
            else
            {
                CurrentScope.AddOperation(
                    new BinaryOperation(CurrentScope, Program)
                    {
                        OperatorMethod = context.op.Type,
                        PoiLocation = sourceLocation,
                        SourceRange = SourceRange(context)
                    });
                CurrentScope.TypeTable.MakeUnary(sourceLocation);
            }
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
                    PoiLocation = SourceLocation(context.op),
                    SourceRange = SourceRange(context)
                });
            else
                CurrentScope.AddOperation(new BinaryOperation(CurrentScope, Program)
                {
                    OperatorMethod = op.Type,
                    PoiLocation = SourceLocation(context.op),
                    SourceRange = SourceRange(context)
                });
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
                    PoiLocation = SourceLocation(context),
                    SourceRange = SourceRange(context)
                });
                break;
            case "INPUT":
                CurrentScope.AddOperation(new InputOperation(CurrentScope, Program)
                {
                    PoiLocation = SourceLocation(context),
                    SourceRange = SourceRange(context)
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
            PoiLocation = SourceLocation(context),
            SourceRange = SourceRange(context)
        });
    }

    public override void ExitWhileStatement(PseudoCodeParser.WhileStatementContext context)
    {
        base.ExitWhileStatement(context);
        var repeatBlock = CurrentScope.TakeLast();
        var testScope = (Scope)CurrentScope.TakeLast();
        CurrentScope.AddOperation(new RepeatOperation(CurrentScope, Program)
        {
            RepeatBlock = repeatBlock, TestExpressionScope = testScope,
            TestFirst = true,
            PoiLocation = SourceLocation(context),
            SourceRange = SourceRange(context)
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
            PoiLocation = SourceLocation(context.scopedExpression().Stop),
            SourceRange = SourceRange(context)
        });
        var repeatBlock = CurrentScope.TakeLast();
        CurrentScope.AddOperation(new RepeatOperation(CurrentScope, Program)
        {
            RepeatBlock = repeatBlock, TestExpressionScope = testScope,
            TestFirst = false,
            PoiLocation = SourceLocation(context),
            SourceRange = SourceRange(context)
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
                Intermediate = CurrentScope.FindType(Type.IntegerId).Instance(1, CurrentScope),
                PoiLocation = SourceLocation(context.Next().Symbol),
                SourceRange = SourceRange(context)
            };
        var target = CurrentScope.TakeLast();
        CurrentScope.AddOperation(new AssignmentOperation(CurrentScope, Program)
        {
            KeepVariableInStack = true,
            PoiLocation = SourceLocation(context.AssignmentNotation().Symbol)
        });
        CurrentScope.AddOperation(new ForOperation(CurrentScope, Program)
        {
            ForBody = body,
            Next = next,
            Step = step,
            TargetValue = target,
            PoiLocation = SourceLocation(context),
            SourceRange = SourceRange(context)
        });
    }

    #region ScopedExpressions

    public override void EnterIndentedBlock(PseudoCodeParser.IndentedBlockContext context)
    {
        base.EnterIndentedBlock(context);
        EnterScope(SourceLocation(context));
    }

    public override void ExitIndentedBlock(PseudoCodeParser.IndentedBlockContext context)
    {
        base.ExitIndentedBlock(context);
        ExitScope(SourceLocation(context.Stop));
    }

    public override void EnterAlignedBlock(PseudoCodeParser.AlignedBlockContext context)
    {
        base.EnterAlignedBlock(context);
        EnterScope(SourceLocation(context));
    }

    public override void ExitAlignedBlock(PseudoCodeParser.AlignedBlockContext context)
    {
        base.ExitAlignedBlock(context);
        ExitScope(SourceLocation(context.Stop));
    }

    public override void EnterScopedExpression(PseudoCodeParser.ScopedExpressionContext context)
    {
        base.EnterScopedExpression(context);
        EnterScope(SourceLocation(context));
    }

    public override void ExitScopedExpression(PseudoCodeParser.ScopedExpressionContext context)
    {
        base.ExitScopedExpression(context);
        ExitScope(SourceLocation(context.Stop));
    }

    #endregion
}
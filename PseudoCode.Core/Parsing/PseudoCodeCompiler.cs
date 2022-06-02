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
        CurrentScope = CurrentScope.AddScope(SourceLocationHelper.SourceLocation(context), SourceLocationHelper.SourceRange(context));
    }

    public void ExitScope(ParserRuleContext context)
    {
        CurrentScope.ParentScope.AddOperation(CurrentScope);
        CurrentScope = CurrentScope.ParentScope;
    }

    public override void ExitDeclarationStatement(PseudoCodeParser.DeclarationStatementContext context)
    {
        base.ExitDeclarationStatement(context);
        // Console.WriteLine($"DECLARE {context.IDENTIFIER().GetText()} : {context.dataType().GetText()}");
        var name = context.Identifier().GetText();
        var type = context.dataType().TypeName;
        var dimensions = context.dataType().Dimensions;
        var sourceLocation = SourceLocationHelper.SourceLocation(context);
        CurrentScope.AddOperation(new DeclareOperation(CurrentScope, Program)
        {
            Name = name,
            PoiLocation = sourceLocation,
            SourceRange = SourceLocationHelper.SourceRange(context)
        });
        var resType = CurrentScope.FindTypeDefinition(type).Type;
        if (dimensions.Count != 0)
        {
            resType = new ArrayType(CurrentScope, Program)
            {
                Dimensions = dimensions,
                ElementType = resType
            };
        }
        CurrentScope.InstanceDefinitions.Add(name, new Definition
        {
            Type = resType,
            Name = name,
            SourceRange = SourceLocationHelper.SourceRange(context.Identifier().Symbol)
        });
    }

    public override void ExitAtom(PseudoCodeParser.AtomContext context)
    {
        base.ExitAtom(context);
        if (context.AtomType == "ARRAY") return; // Let array handle on its own
        CurrentScope.AddOperation(new LoadImmediateOperation(CurrentScope, Program)
        {
            Intermediate = CurrentScope.FindTypeDefinition(context.AtomType).Type.Instance(context.Value, CurrentScope),
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
                SourceRange = SourceLocationHelper.SourceRange(context)
            });
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
        CurrentScope.AddOperation(new RepeatOperation(CurrentScope, Program)
        {
            RepeatBlock = repeatBlock, TestExpressionScope = testScope,
            TestFirst = true,
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
        var repeatBlock = CurrentScope.TakeLast();
        CurrentScope.AddOperation(new RepeatOperation(CurrentScope, Program)
        {
            RepeatBlock = repeatBlock, TestExpressionScope = testScope,
            TestFirst = false,
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
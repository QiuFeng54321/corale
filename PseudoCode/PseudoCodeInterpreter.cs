using Antlr4.Runtime;
using PseudoCode.Runtime;
using PseudoCode.Runtime.Operations;

namespace PseudoCode;

public class PseudoCodeInterpreter : PseudoCodeBaseListener
{
    public Scope CurrentScope;
    public PseudoProgram Program;

    public override void EnterFileInput(PseudoCodeParser.FileInputContext context)
    {
        base.EnterFileInput(context);
        Program = new PseudoProgram();
        CurrentScope = Program.GlobalScope;
    }

    public override void ExitFileInput(PseudoCodeParser.FileInputContext context)
    {
        base.ExitFileInput(context);
        Console.WriteLine(Program.GlobalScope);
        Console.WriteLine("Operate...");
        Program.GlobalScope.Operate();
    }

    public void EnterScope(SourceLocation sourceLocation = default)
    {
        CurrentScope = CurrentScope.AddScope(sourceLocation);
    }

    public void ExitScope()
    {
        CurrentScope.ParentScope.AddOperation(CurrentScope);
        CurrentScope = CurrentScope.ParentScope;
    }

    public override void ExitDeclarationStatement(PseudoCodeParser.DeclarationStatementContext context)
    {
        base.ExitDeclarationStatement(context);
        // Console.WriteLine($"DECLARE {context.IDENTIFIER().GetText()} : {context.dataType().GetText()}");
        CurrentScope.AddOperation(new DeclareOperation(CurrentScope, Program)
        {
            Name = context.Identifier().GetText(),
            Type = CurrentScope.FindType(context.dataType().TypeName),
            Dimensions = context.dataType().Dimensions,
            SourceLocation = SourceLocation(context)
        });
    }

    private static SourceLocation SourceLocation(IToken token)
    {
        return token == null ? null : new SourceLocation(token.Line, token.Column + 1);
    }

    private static SourceLocation SourceLocation(ParserRuleContext context)
    {
        return SourceLocation(context.Start);
    }

    public override void ExitAtom(PseudoCodeParser.AtomContext context)
    {
        base.ExitAtom(context);
        if (context.AtomType == "ARRAY") return; // Let array handle on its own
        CurrentScope.AddOperation(new LoadImmediateOperation(CurrentScope, Program)
        {
            Intermediate = CurrentScope.FindType(context.AtomType).Instance(context.Value),
            SourceLocation = SourceLocation(context)
        });
    }

    public override void ExitArray(PseudoCodeParser.ArrayContext context)
    {
        base.ExitArray(context);
        CurrentScope.AddOperation(new FormImmediateArrayOperation(CurrentScope, Program)
        {
            Length = context.expression().Length,
            SourceLocation = SourceLocation(context)
        });
    }

    public override void ExitAssignmentStatement(PseudoCodeParser.AssignmentStatementContext context)
    {
        base.ExitAssignmentStatement(context);
        // Console.WriteLine($"{context.lvalue().GetText()} <- {context.expr().GetText()}");
        CurrentScope.AddOperation(new AssignmentOperation(CurrentScope, Program)
        {
            SourceLocation = SourceLocation(context)
        });
    }

    public override void ExitArithmeticExpression(PseudoCodeParser.ArithmeticExpressionContext context)
    {
        base.ExitArithmeticExpression(context);

        if (context.array() != null)
            CurrentScope.AddOperation(new ArrayIndexOperation(CurrentScope, Program)
            {
                SourceLocation = SourceLocation(context.array())
            });
        else if (context.Identifier() != null && context.IsUnary)
            CurrentScope.AddOperation(new LoadOperation(CurrentScope, Program)
            {
                LoadName = context.Identifier().GetText(),
                SourceLocation = SourceLocation(context)
            });
        if (context.op != null)
            if (context.IsUnary)
                // TODO ambiguous operator with Caret
                CurrentScope.AddOperation(new UnaryOperation(CurrentScope, Program)
                {
                    OperatorMethod = context.op.Type,
                    SourceLocation = SourceLocation(context.op)
                });
            else
                CurrentScope.AddOperation(
                    new BinaryOperation(CurrentScope, Program)
                    {
                        OperatorMethod = context.op.Type,
                        SourceLocation = SourceLocation(context.op)
                    });
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
                    SourceLocation = SourceLocation(context.op)
                });
            else
                CurrentScope.AddOperation(new BinaryOperation(CurrentScope, Program)
                {
                    OperatorMethod = op.Type,
                    SourceLocation = SourceLocation(context.op)
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
                    SourceLocation = SourceLocation(context)
                });
                break;
            case "INPUT":
                CurrentScope.AddOperation(new InputOperation(CurrentScope, Program)
                {
                    SourceLocation = SourceLocation(context)
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
            SourceLocation = SourceLocation(context)
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
            SourceLocation = SourceLocation(context)
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
            SourceLocation = SourceLocation(context.scopedExpression().Stop)
        });
        var repeatBlock = CurrentScope.TakeLast();
        CurrentScope.AddOperation(new RepeatOperation(CurrentScope, Program)
        {
            RepeatBlock = repeatBlock, TestExpressionScope = testScope,
            TestFirst = false,
            SourceLocation = SourceLocation(context)
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
        ExitScope();
    }

    public override void EnterAlignedBlock(PseudoCodeParser.AlignedBlockContext context)
    {
        base.EnterAlignedBlock(context);
        EnterScope(SourceLocation(context));
    }

    public override void ExitAlignedBlock(PseudoCodeParser.AlignedBlockContext context)
    {
        base.ExitAlignedBlock(context);
        ExitScope();
    }

    public override void EnterScopedExpression(PseudoCodeParser.ScopedExpressionContext context)
    {
        base.EnterScopedExpression(context);
        EnterScope(SourceLocation(context));
    }

    public override void ExitScopedExpression(PseudoCodeParser.ScopedExpressionContext context)
    {
        base.ExitScopedExpression(context);
        ExitScope();
    }

    #endregion
}
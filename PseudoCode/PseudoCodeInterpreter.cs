using Antlr4.Runtime;
using PseudoCode.Runtime;
using PseudoCode.Runtime.Operations;

namespace PseudoCode;

public class PseudoCodeInterpreter : PseudoCodeBaseListener
{
    public PseudoProgram Program;
    public Scope CurrentScope;
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

    public void EnterScope() => CurrentScope = CurrentScope.AddScope();

    public void ExitScope()
    {
        CurrentScope.ParentScope.AddOperation(CurrentScope);
        CurrentScope = CurrentScope.ParentScope;
    }

    #region ScopedExpressions

    public override void EnterIndentedBlock(PseudoCodeParser.IndentedBlockContext context)
    {
        base.EnterIndentedBlock(context);
        EnterScope();
    }

    public override void ExitIndentedBlock(PseudoCodeParser.IndentedBlockContext context)
    {
        base.ExitIndentedBlock(context);
        ExitScope();
    }

    public override void EnterAlignedBlock(PseudoCodeParser.AlignedBlockContext context)
    {
        base.EnterAlignedBlock(context);
        EnterScope();
    }

    public override void ExitAlignedBlock(PseudoCodeParser.AlignedBlockContext context)
    {
        base.ExitAlignedBlock(context);
        ExitScope();
    }

    public override void EnterScopedExpression(PseudoCodeParser.ScopedExpressionContext context)
    {
        base.EnterScopedExpression(context);
        EnterScope();
    }

    public override void ExitScopedExpression(PseudoCodeParser.ScopedExpressionContext context)
    {
        base.ExitScopedExpression(context);
        ExitScope();
    }

    #endregion

    public override void ExitDeclarationStatement(PseudoCodeParser.DeclarationStatementContext context)
    {
        base.ExitDeclarationStatement(context);
        // Console.WriteLine($"DECLARE {context.IDENTIFIER().GetText()} : {context.dataType().GetText()}");
        CurrentScope.AddOperation(new DeclareOperation (CurrentScope, Program)
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

    public override void ExitLvalue(PseudoCodeParser.LvalueContext context)
    {
        base.ExitLvalue(context);
        // TODO
        CurrentScope.AddOperation(new LoadOperation (CurrentScope, Program)
        {
            LoadName = context.Identifier().GetText(),
            SourceLocation = SourceLocation(context)
        });
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
                // TODO ambiguous operator with Caret
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
        if (context.IoKeyword().GetText() == "OUTPUT")
            CurrentScope.AddOperation(new OutputOperation(CurrentScope, Program)
            {
                ArgumentCount = context.tuple().expression().Length,
                SourceLocation = SourceLocation(context)
            });
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
}
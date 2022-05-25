using PseudoCode.Runtime;
using PseudoCode.Runtime.Operations;

namespace PseudoCode;

public class PseudoCodeInterpreter : PseudoCodeBaseListener
{
    public Scope CurrentScope;
    public Scope GlobalScope = new();

    public override void EnterFileInput(PseudoCodeParser.FileInputContext context)
    {
        base.EnterFileInput(context);
        CurrentScope = GlobalScope;
        GlobalScope.AddType("BOOLEAN", new BooleanType());
        GlobalScope.AddType("INTEGER", new IntegerType());
        GlobalScope.AddType("REAL", new RealType());
        GlobalScope.AddType("ARRAY", new ArrayType());
    }

    public override void ExitFileInput(PseudoCodeParser.FileInputContext context)
    {
        base.ExitFileInput(context);
        Console.WriteLine(GlobalScope);
        Console.WriteLine("Operate...");
        GlobalScope.Operate();
    }

    public override void EnterBlock(PseudoCodeParser.BlockContext context)
    {
        base.EnterBlock(context);
        CurrentScope = CurrentScope.AddScope();
    }

    public override void ExitBlock(PseudoCodeParser.BlockContext context)
    {
        base.ExitBlock(context);
        CurrentScope.Parent.AddOperation(CurrentScope);
        CurrentScope = CurrentScope.Parent;
    }

    public override void ExitDeclarationStatement(PseudoCodeParser.DeclarationStatementContext context)
    {
        base.ExitDeclarationStatement(context);
        // Console.WriteLine($"DECLARE {context.IDENTIFIER().GetText()} : {context.dataType().GetText()}");
        CurrentScope.AddOperation(new DeclareOperation
        {
            Scope = CurrentScope,
            Name = context.Identifier().GetText(),
            Type = CurrentScope.FindType(context.dataType().TypeName),
            Dimensions = context.dataType().Dimensions
        });
    }

    public override void ExitLvalue(PseudoCodeParser.LvalueContext context)
    {
        base.ExitLvalue(context);
        // TODO
        CurrentScope.AddOperation(new LoadOperation
            { Scope = CurrentScope, LoadName = context.Identifier().GetText() });
    }

    public override void ExitAtom(PseudoCodeParser.AtomContext context)
    {
        base.ExitAtom(context);
        CurrentScope.AddOperation(new LoadImmediateOperation
            { Scope = CurrentScope, Intermediate = CurrentScope.FindType(context.AtomType).Instance(context.Value) });
    }

    public override void ExitAssignmentStatement(PseudoCodeParser.AssignmentStatementContext context)
    {
        base.ExitAssignmentStatement(context);
        // Console.WriteLine($"{context.lvalue().GetText()} <- {context.expr().GetText()}");
        CurrentScope.AddOperation(new AssignmentOperation { Scope = CurrentScope });
    }

    public override void ExitArithmeticExpression(PseudoCodeParser.ArithmeticExpressionContext context)
    {
        base.ExitArithmeticExpression(context);
        if (context.op != null)
            if (context.IsUnary)
                // TODO ambiguous operator with Caret
                CurrentScope.AddOperation(new UnaryOperation{Scope = CurrentScope, OperatorMethod = context.op.Type});
            else 
                CurrentScope.AddOperation(new BinaryOperation { Scope = CurrentScope, OperatorMethod = context.op.Type});
    }

    public override void ExitLogicExpression(PseudoCodeParser.LogicExpressionContext context)
    {
        base.ExitLogicExpression(context);
        var op = context.op ?? context.comp?.Start;
        if (op != null)
            if (context.IsUnary)
                // TODO ambiguous operator with Caret
                CurrentScope.AddOperation(new UnaryOperation{Scope = CurrentScope, OperatorMethod = op.Type});
            else 
                CurrentScope.AddOperation(new BinaryOperation { Scope = CurrentScope, OperatorMethod = op.Type});

    }

    public override void ExitIoStatement(PseudoCodeParser.IoStatementContext context)
    {
        base.ExitIoStatement(context);
        // Console.WriteLine($"{context.IO_KEYWORD()} {context.expression().GetText()}");
        if (context.IoKeyword().GetText() == "OUTPUT")
            CurrentScope.AddOperation(new OutputOperation { Scope = CurrentScope, ArgumentCount = context.tuple().expression().Length});
    }

    public override void ExitIfStatement(PseudoCodeParser.IfStatementContext context)
    {
        base.ExitIfStatement(context);
        var elseBlock = context.HasElse ? CurrentScope.TakeLast() : null;
        var ifBlock = CurrentScope.TakeLast();
        CurrentScope.AddOperation(new IfOperation{Scope = CurrentScope, FalseBlock = elseBlock, TrueBlock = ifBlock});
    }
    
}
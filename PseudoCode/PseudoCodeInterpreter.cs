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
        CurrentScope.Parent.Operations.Enqueue(CurrentScope);
        CurrentScope = CurrentScope.Parent;
    }

    public override void ExitDeclarationStatement(PseudoCodeParser.DeclarationStatementContext context)
    {
        base.ExitDeclarationStatement(context);
        // Console.WriteLine($"DECLARE {context.IDENTIFIER().GetText()} : {context.dataType().GetText()}");
        CurrentScope.Operations.Enqueue(new DeclareOperation
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
        CurrentScope.Operations.Enqueue(new LoadOperation
            { Scope = CurrentScope, LoadName = context.Identifier().GetText() });
    }

    public override void ExitAtom(PseudoCodeParser.AtomContext context)
    {
        base.ExitAtom(context);
        CurrentScope.Operations.Enqueue(new LoadImmediateOperation
            { Scope = CurrentScope, Intermediate = CurrentScope.FindType(context.AtomType).Instance(context.Value) });
    }

    public override void ExitAssignmentStatement(PseudoCodeParser.AssignmentStatementContext context)
    {
        base.ExitAssignmentStatement(context);
        // Console.WriteLine($"{context.lvalue().GetText()} <- {context.expr().GetText()}");
        CurrentScope.Operations.Enqueue(new AssignmentOperation { Scope = CurrentScope });
    }

    public override void ExitArithmeticExpression(PseudoCodeParser.ArithmeticExpressionContext context)
    {
        base.ExitArithmeticExpression(context);
        if (context.op != null)
            if (context.IsUnary)
                // TODO ambiguous operator with Caret
                CurrentScope.Operations.Enqueue(new UnaryOperation{Scope = CurrentScope, OperatorMethod = context.op.Type});
            else 
                CurrentScope.Operations.Enqueue(new BinaryOperation { Scope = CurrentScope, OperatorMethod = context.op.Type});
    }

    public override void ExitIoStatement(PseudoCodeParser.IoStatementContext context)
    {
        base.ExitIoStatement(context);
        // Console.WriteLine($"{context.IO_KEYWORD()} {context.expression().GetText()}");
        if (context.IoKeyword().GetText() == "OUTPUT")
            CurrentScope.Operations.Enqueue(new OutputOperation { Scope = CurrentScope, ArgumentCount = context.tuple().expression().Length});
    }

    public override void EnterIfStatement(PseudoCodeParser.IfStatementContext context)
    {
        base.EnterIfStatement(context);
        Console.WriteLine($"if {context.expression().GetText()}");
    }
}
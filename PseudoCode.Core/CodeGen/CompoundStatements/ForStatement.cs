using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.CodeGen.Expressions;
using PseudoCode.Core.CodeGen.SimpleStatements;
using PseudoCode.Core.Formatting;
using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.CodeGen.CompoundStatements;

public class ForStatement : Statement
{
    public Block Block;
    public AssignmentStatement Initial;
    public Expression Next;
    public Expression Target, Step;

    public override void Format(PseudoFormatter formatter)
    {
        formatter.WriteStatement($"FOR {Initial} TO {Target}" + (Step != null ? $" STEP {Step}" : ""));
        Block.Format(formatter);
        formatter.WriteStatement($"NEXT {Next}");
    }

    public override void CodeGen(CodeGenContext ctx, CompilationUnit cu, Function function)
    {
        Initial.CodeGen(ctx, cu, function);
        var conditionBlock = function.LLVMFunction.AppendBasicBlock(ReservedNames.Condition);
        var continueBlock = function.LLVMFunction.AppendBasicBlock(ReservedNames.BlockRefContinuation);
        var bodyBlock = function.LLVMFunction.AppendBasicBlock(ReservedNames.Block);
        var incrementBlock = function.LLVMFunction.AppendBasicBlock(ReservedNames.Increment);
        cu.Builder.BuildBr(conditionBlock);
        cu.Builder.PositionAtEnd(conditionBlock);
        function.CurrentBlockRef = conditionBlock;
        var topCmpExpr = GenerateTopCmpExpr();

        var bottomCmpExpr = new BinaryExpression
        {
            Left = Initial.Target,
            Right = Target,
            DebugInformation = DebugInformation,
            Operator = PseudoOperator.Equal
        };
        var cmpSym = topCmpExpr.CodeGen(ctx, cu, function);
        cu.Builder.BuildCondBr(cmpSym.GetRealValueRef(ctx, cu), continueBlock, bodyBlock);
        cu.Builder.PositionAtEnd(bodyBlock);
        function.CurrentBlockRef = bodyBlock;
        Block.CodeGen(ctx, cu, function);


        var cmpSym2 = bottomCmpExpr.CodeGen(ctx, cu, function);
        cu.Builder.BuildCondBr(cmpSym2.GetRealValueRef(ctx, cu), continueBlock, incrementBlock);
        function.CurrentBlockRef = incrementBlock;
        cu.Builder.PositionAtEnd(incrementBlock);


        var incrementExpr = GenerateNextStatement();
        incrementExpr.CodeGen(ctx, cu, function);
        cu.Builder.BuildBr(bodyBlock);
        function.CurrentBlockRef = continueBlock;
        cu.Builder.PositionAtEnd(continueBlock);
    }

    private BinaryExpression GenerateTopCmpExpr()
    {
        // (Step > 0 && Initial > Value) || (Step <= 0 && Initial < Value)
        return new BinaryExpression
        {
            Left = new BinaryExpression
            {
                Left = Step == null
                    ? new PseudoBoolean { Value = true }
                    : new BinaryExpression
                    {
                        Left = Step,
                        Right = new PseudoInteger { Value = 0 },
                        Operator = PseudoOperator.Greater
                    },
                Right = new BinaryExpression
                {
                    Left = Initial.Target,
                    Right = Target,
                    Operator = PseudoOperator.Greater
                },
                Operator = PseudoOperator.And
            },
            Right = new BinaryExpression
            {
                Left = Step == null
                    ? new PseudoBoolean { Value = false }
                    : new BinaryExpression
                    {
                        Left = Step,
                        Right = new PseudoInteger { Value = 0 },
                        Operator = PseudoOperator.SmallerEqual
                    },
                Right = new BinaryExpression
                {
                    Left = Initial.Target,
                    Right = Target,
                    Operator = PseudoOperator.Smaller
                },
                Operator = PseudoOperator.And
            },
            DebugInformation = DebugInformation,
            Operator = PseudoOperator.Or
        };
    }

    private AssignmentStatement GenerateNextStatement()
    {
        return new AssignmentStatement
        {
            Target = Next,
            Value = new BinaryExpression
            {
                Left = Next,
                Right = Step ?? new PseudoInteger { Value = 1 },
                Operator = PseudoOperator.Add,
                DebugInformation = DebugInformation
            },
            DebugInformation = DebugInformation
        };
    }
}
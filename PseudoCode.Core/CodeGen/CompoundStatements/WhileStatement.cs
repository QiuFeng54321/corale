using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.Formatting;

namespace PseudoCode.Core.CodeGen.CompoundStatements;

public class WhileStatement : Statement
{
    public Expression Condition;
    public Block Then;

    public override void Format(PseudoFormatter formatter)
    {
        formatter.WriteStatement($"WHILE {Condition}");
        Then.Format(formatter);
        formatter.WriteStatement("ENDWHILE");
    }

    public override void CodeGen(CodeGenContext ctx, CompilationUnit cu, Function function)
    {
        var conditionBlockRef = function.LLVMFunction.AppendBasicBlock(ReservedNames.Condition);
        cu.Builder.BuildBr(conditionBlockRef);
        cu.Builder.PositionAtEnd(conditionBlockRef);
        var cond = Condition.CodeGen(ctx, cu, function);
        var thenBlockRef = function.LLVMFunction.AppendBasicBlock(ReservedNames.Then);
        var continueBlockRef = function.LLVMFunction.AppendBasicBlock(ReservedNames.BlockRefContinuation);
        cu.Builder.BuildCondBr(cond.GetRealValueRef(ctx, cu), thenBlockRef, continueBlockRef);
        // Then
        cu.Builder.PositionAtEnd(thenBlockRef);
        function.CurrentBlockRef = thenBlockRef;
        Then.CodeGen(ctx, cu, function);
        cu.Builder.BuildBr(conditionBlockRef); // Jump to condition

        function.CurrentBlockRef = continueBlockRef;
        cu.Builder.PositionAtEnd(continueBlockRef);
    }
}
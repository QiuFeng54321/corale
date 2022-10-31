using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.Formatting;

namespace PseudoCode.Core.CodeGen.CompoundStatements;

public class RepeatStatement : Statement
{
    public Expression Condition;
    public Block Then;

    public override void Format(PseudoFormatter formatter)
    {
        formatter.WriteStatement("REPEAT");
        Then.Format(formatter);
        formatter.WriteStatement($"UNTIL {Condition}");
    }

    public override void CodeGen(CodeGenContext ctx, CompilationUnit cu, Function function)
    {
        var thenBlockRef = function.LLVMFunction.AppendBasicBlock(ReservedNames.Then);
        cu.Builder.BuildBr(thenBlockRef);
        var continueBlockRef = function.LLVMFunction.AppendBasicBlock(ReservedNames.BlockRefContinuation);
        // Then
        cu.Builder.PositionAtEnd(thenBlockRef);
        function.CurrentBlockRef = thenBlockRef;
        Then.CodeGen(ctx, cu, function);
        // Until
        var cond = Condition.CodeGen(ctx, cu, function);
        cu.Builder.BuildCondBr(cond.GetRealValueRef(ctx, cu), continueBlockRef, thenBlockRef); // Jump to condition
        // Continue
        function.CurrentBlockRef = continueBlockRef;
        cu.Builder.PositionAtEnd(continueBlockRef);
    }
}
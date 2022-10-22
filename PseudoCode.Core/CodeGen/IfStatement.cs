using LLVMSharp.Interop;
using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.Formatting;

namespace PseudoCode.Core.CodeGen;

public class IfStatement : Statement
{
    public Expression Condition;
    public Block Else;
    public Block Then;

    public override void Format(PseudoFormatter formatter)
    {
        formatter.WriteStatement($"IF {Condition.ToFormatString()} THEN");
        Then.Format(formatter);
        if (Else != null)
        {
            formatter.WriteStatement("ELSE");
            Else.Format(formatter);
        }

        formatter.WriteStatement("ENDIF");
    }

    public override unsafe void CodeGen(CodeGenContext ctx, Function function)
    {
        var condRes = Condition.CodeGen(ctx, function);
        // var function = LLVM.GetBasicBlockParent(LLVM.GetInsertBlock(ctx.Builder));
        var thenBlockRef = function.LLVMFunction.AppendBasicBlock(ReservedNames.Then);
        var elseBlockRef = Else == null ? null : function.LLVMFunction.AppendBasicBlock(ReservedNames.Else);
        var continueBlockRef = function.LLVMFunction.AppendBasicBlock(ReservedNames.BlockRefContinuation);

        LLVM.BuildCondBr(ctx.Builder, condRes.GetRealValueRef(ctx), thenBlockRef,
            Else != null ? elseBlockRef : continueBlockRef);

        // Emit then value.
        ctx.Builder.PositionAtEnd(thenBlockRef);
        Then.CodeGen(ctx, function);
        ctx.Builder.BuildBr(continueBlockRef);

        if (Else != null)
        {
            ctx.Builder.PositionAtEnd(elseBlockRef);
            Else.CodeGen(ctx, function);
            ctx.Builder.BuildBr(continueBlockRef);
        }

        function.CurrentBlockRef = continueBlockRef;
        ctx.Builder.PositionAtEnd(continueBlockRef);
    }
}
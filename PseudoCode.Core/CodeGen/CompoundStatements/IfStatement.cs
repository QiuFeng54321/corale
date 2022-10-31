using LLVMSharp.Interop;
using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.Formatting;

namespace PseudoCode.Core.CodeGen.CompoundStatements;

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

    public override unsafe void CodeGen(CodeGenContext ctx, CompilationUnit cu, Function function)
    {
        var condRes = Condition.CodeGen(ctx, cu, function);
        // var function = LLVM.GetBasicBlockParent(LLVM.GetInsertBlock(cu.Builder));
        var thenBlockRef = function.LLVMFunction.AppendBasicBlock(ReservedNames.Then);
        var elseBlockRef = Else == null ? null : function.LLVMFunction.AppendBasicBlock(ReservedNames.Else);
        var continueBlockRef = function.LLVMFunction.AppendBasicBlock(ReservedNames.BlockRefContinuation);

        LLVM.BuildCondBr(cu.Builder, condRes.GetRealValueRef(ctx, cu), thenBlockRef,
            Else != null ? elseBlockRef : continueBlockRef);

        // Emit then value.
        cu.Builder.PositionAtEnd(thenBlockRef);
        function.CurrentBlockRef = thenBlockRef;
        Then.CodeGen(ctx, cu, function);
        cu.Builder.BuildBr(continueBlockRef);

        if (Else != null)
        {
            cu.Builder.PositionAtEnd(elseBlockRef);
            function.CurrentBlockRef = elseBlockRef;
            Else.CodeGen(ctx, cu, function);
            cu.Builder.BuildBr(continueBlockRef);
        }

        function.CurrentBlockRef = continueBlockRef;
        cu.Builder.PositionAtEnd(continueBlockRef);
    }
}
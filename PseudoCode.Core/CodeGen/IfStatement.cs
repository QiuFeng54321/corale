using LLVMSharp.Interop;
using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.Formatting;

namespace PseudoCode.Core.CodeGen;

public class IfStatement : Statement
{
    public Expression Condition;
    public Block Continue;
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

    public override unsafe void CodeGen(CodeGenContext ctx, Block block)
    {
        var condRes = Condition.CodeGen(ctx, block);
        // var function = LLVM.GetBasicBlockParent(LLVM.GetInsertBlock(ctx.Builder));
        Then.InitializeBlock();
        Else?.InitializeBlock();
        Continue.InitializeBlock();

        LLVM.BuildCondBr(ctx.Builder, condRes.GetRealValueRef(ctx), Then.BlockRef, Else?.BlockRef ?? Continue.BlockRef);

        // Emit then value.
        Then.CodeGen(ctx, block);
        ctx.Builder.BuildBr(Continue.BlockRef);

        if (Else != null)
        {
            Else.CodeGen(ctx, block);
            ctx.Builder.BuildBr(Continue.BlockRef);
        }
    }
}
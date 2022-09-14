namespace PseudoCode.Core.CodeGen;

public class AssignmentStatement : Statement
{
    public Expression Target, Value;

    public override void CodeGen(CodeGenContext ctx, Block block)
    {
        var val = Value.CodeGen(ctx, block).GetRealValueRef(ctx);
        var target = Target.CodeGen(ctx, block).MemoryPointer;
        ctx.Builder.BuildStore(val, target);
    }
}
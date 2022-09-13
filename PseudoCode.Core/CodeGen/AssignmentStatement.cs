namespace PseudoCode.Core.CodeGen;

public class AssignmentStatement : Statement
{
    public Expression Target, Value;

    public override void CodeGen(CodeGenContext ctx, Block block)
    {
        ctx.Builder.BuildStore(Value.CodeGen(ctx, block).ValueRef, Target.CodeGen(ctx, block).ValueRef);
    }
}
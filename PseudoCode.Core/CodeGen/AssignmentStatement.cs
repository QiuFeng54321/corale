namespace PseudoCode.Core.CodeGen;

public class AssignmentStatement : Statement
{
    public Expression Target, Value;

    public override void CodeGen(CodeGenContext ctx, Block block)
    {
        var val = Value.CodeGen(ctx, block).GetRealValue(ctx).ValueRef;
        var target = Target.CodeGen(ctx, block).Alloca;
        ctx.Builder.BuildStore(val, target);
    }
}
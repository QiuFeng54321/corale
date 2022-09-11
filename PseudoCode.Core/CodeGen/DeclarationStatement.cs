namespace PseudoCode.Core.CodeGen;

public class DeclarationStatement : Statement
{
    public Symbol Symbol;

    public override void CodeGen(CodeGenContext ctx)
    {
        Symbol.ValueRef = ctx.IRBuilder.BuildMalloc(Symbol.Type.GetLLVMType(), Symbol.Name);
    }
}
using LLVMSharp.Interop;

namespace PseudoCode.Core.CodeGen;

public class PseudoInteger : Expression
{
    public int Value;

    public override Symbol CodeGen(CodeGenContext ctx, Block block)
    {
        var val = LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, (ulong)Value);
        return Symbol.MakeTemp(ReservedNames.Integer, "INTEGER", ctx, val);
    }
}
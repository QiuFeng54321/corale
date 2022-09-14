using LLVMSharp.Interop;

namespace PseudoCode.Core.CodeGen;

public class PseudoBoolean : Expression
{
    public bool Value;

    public override Symbol CodeGen(CodeGenContext ctx, Block block)
    {
        var val = LLVMValueRef.CreateConstInt(LLVMTypeRef.Int1, Value ? 1ul : 0);
        return Symbol.MakeTemp(ReservedNames.Integer, BuiltinTypes.Integer.Type, ctx, val);
    }
}
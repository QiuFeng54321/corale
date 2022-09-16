using System.Globalization;
using LLVMSharp.Interop;

namespace PseudoCode.Core.CodeGen;

public class PseudoReal : Expression
{
    public double Value;

    public override Symbol CodeGen(CodeGenContext ctx, Block block)
    {
        var val = LLVMValueRef.CreateConstReal(LLVMTypeRef.Double, Value);
        return Symbol.MakeTemp(ReservedNames.Real, BuiltinTypes.Real.Type, ctx, val);
    }

    public override string ToFormatString()
    {
        return Value.ToString(CultureInfo.CurrentCulture);
    }
}
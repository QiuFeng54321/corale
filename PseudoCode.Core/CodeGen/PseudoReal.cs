using System.Globalization;
using LLVMSharp.Interop;
using PseudoCode.Core.CodeGen.Containers;

namespace PseudoCode.Core.CodeGen;

public class PseudoReal : Expression
{
    public double Value;

    public override Symbol CodeGen(CodeGenContext ctx, CompilationUnit cu, Function function)
    {
        var val = LLVMValueRef.CreateConstReal(LLVMTypeRef.Double, Value);
        return Symbol.MakeTemp(ReservedNames.Real, BuiltinTypes.Real.Type, ctx, val);
    }

    public override string ToFormatString()
    {
        return Value.ToString(CultureInfo.CurrentCulture);
    }
}
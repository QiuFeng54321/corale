using LLVMSharp.Interop;
using PseudoCode.Core.CodeGen.Containers;

namespace PseudoCode.Core.CodeGen.Expressions;

public class PseudoInteger : Expression
{
    public long Value;

    public override Symbol CodeGen(CodeGenContext ctx, CompilationUnit cu, Function function)
    {
        var val = LLVMValueRef.CreateConstInt(LLVMTypeRef.Int64, (ulong)Value);
        return Symbol.MakeTemp(ReservedNames.Integer, BuiltinTypes.Integer.Type, ctx, val);
    }

    public override string ToFormatString()
    {
        return Value.ToString();
    }
}
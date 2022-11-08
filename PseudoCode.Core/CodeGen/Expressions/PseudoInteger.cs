using LLVMSharp.Interop;
using PseudoCode.Core.CodeGen.Containers;

namespace PseudoCode.Core.CodeGen.Expressions;

public class PseudoInteger : Expression
{
    public static readonly LLVMValueRef One = LLVMValueRef.CreateConstInt(LLVMTypeRef.Int64, 1);
    public static readonly LLVMValueRef Zero = LLVMValueRef.CreateConstInt(LLVMTypeRef.Int64, 0);
    public long Value;

    public override Symbol CodeGen(CodeGenContext ctx, CompilationUnit cu, Function function)
    {
        var val = LLVMValueRef.CreateConstInt(LLVMTypeRef.Int64, (ulong)Value);
        return Symbol.MakeTemp(ReservedNames.Integer, BuiltinTypes.Integer.Type, ctx, val, Value);
    }

    public override string ToFormatString()
    {
        return Value.ToString();
    }
}
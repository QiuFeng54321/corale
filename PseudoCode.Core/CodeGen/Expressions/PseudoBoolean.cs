using LLVMSharp.Interop;
using PseudoCode.Core.CodeGen.Containers;

namespace PseudoCode.Core.CodeGen.Expressions;

public class PseudoBoolean : Expression
{
    public bool Value;

    public override Symbol CodeGen(CodeGenContext ctx, CompilationUnit cu, Function function)
    {
        var val = LLVMValueRef.CreateConstInt(LLVMTypeRef.Int1, Value ? 1ul : 0);
        return Symbol.MakeTemp(ReservedNames.Boolean, BuiltinTypes.Boolean.Type, ctx, val);
    }

    public override string ToFormatString()
    {
        return Value ? "TRUE" : "FALSE";
    }
}
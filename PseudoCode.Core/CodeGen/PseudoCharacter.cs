using LLVMSharp.Interop;
using PseudoCode.Core.CodeGen.Containers;

namespace PseudoCode.Core.CodeGen;

public class PseudoCharacter : Expression
{
    public char Value;

    public override Symbol CodeGen(CodeGenContext ctx, Function function)
    {
        var val = LLVMValueRef.CreateConstInt(LLVMTypeRef.Int8, Value);
        return Symbol.MakeTemp(ReservedNames.Char, BuiltinTypes.Char.Type, ctx, val);
    }

    public override string ToFormatString()
    {
        return '\'' + Value.ToString() + '\'';
    }
}
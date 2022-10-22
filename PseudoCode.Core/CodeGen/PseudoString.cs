using System.Text.RegularExpressions;
using LLVMSharp.Interop;
using PseudoCode.Core.CodeGen.Containers;

namespace PseudoCode.Core.CodeGen;

public class PseudoString : Expression
{
    public string Value;

    public override Symbol CodeGen(CodeGenContext ctx, Function function)
    {
        var name = ctx.NameGenerator.Request(ReservedNames.String);
        LLVMValueRef val;
        unsafe
        {
            val = LLVM.BuildGlobalStringPtr(ctx.Builder, Value.ToSByte(), name.ToSByte());
        }

        return Symbol.MakeTemp(BuiltinTypes.CharPtr.Type, val);
    }

    public override string ToFormatString()
    {
        return $"\"{Regex.Escape(Value)}\"";
    }
}
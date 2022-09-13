using System.Runtime.InteropServices;
using LLVMSharp.Interop;

namespace PseudoCode.Core.CodeGen;

public class PseudoString : Expression
{
    public string Value;

    public override Symbol CodeGen(CodeGenContext ctx, Block block)
    {
        var name = ctx.NameGenerator.Request(ReservedNames.String);
        LLVMValueRef val;
        unsafe
        {
            val = LLVM.BuildGlobalStringPtr(ctx.Builder, ToSByte(Value), ToSByte(name));
        }

        return Symbol.MakeTemp(ReservedNames.String, "__CSTRING", ctx, val);
    }

    public static unsafe sbyte* ToSByte(string str)
    {
        return (sbyte*)Marshal.StringToHGlobalAuto(str);
    }
}
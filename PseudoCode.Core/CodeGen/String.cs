using System.Runtime.InteropServices;
using LLVMSharp.Interop;

namespace PseudoCode.Core.CodeGen;

public class String : Expression
{
    public string Value;

    public override LLVMValueRef CodeGen(CodeGenContext ctx)
    {
        var name = ctx.NameGenerator.Request(ReservedNames.String);
        unsafe
        {
            return LLVM.BuildGlobalStringPtr(ctx.Builder, ToSByte(Value), ToSByte(name));
        }
    }

    public static unsafe sbyte* ToSByte(string str)
    {
        return (sbyte*)Marshal.StringToHGlobalAuto(str);
    }
}
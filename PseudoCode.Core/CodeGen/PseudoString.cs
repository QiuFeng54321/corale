using System.Text.RegularExpressions;
using LLVMSharp.Interop;
using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.Runtime.Reflection.Builtin;

namespace PseudoCode.Core.CodeGen;

public class PseudoString : Expression
{
    public string Value;

    public override Symbol CodeGen(CodeGenContext ctx, Function function)
    {
        var name = ctx.NameGenerator.Request(ReservedNames.String);
        LLVMValueRef val;
        PseudoStringStruct str = Value;
        unsafe
        {
            val = LLVM.BuildGlobalStringPtr(ctx.Builder, str.Pointer, name.ToSByte());
        }

        val = LLVMValueRef.CreateConstNamedStruct(BuiltinTypes.String.Type.GetLLVMType(),
            new[] { val, LLVMValueRef.CreateConstInt(BuiltinTypes.Integer.Type.GetLLVMType(), (ulong)str.Length) });
        return Symbol.MakeTemp(BuiltinTypes.String.Type, val);
    }

    public override string ToFormatString()
    {
        return $"\"{Regex.Escape(Value)}\"";
    }
}
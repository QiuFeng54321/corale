using LLVMSharp.Interop;
using PseudoCode.Core.Parsing;

namespace PseudoCode.Core.CodeGen;

public class Integer : Expression
{
    public int Value;

    public override LLVMValueRef CodeGen(CodeGenContext ctx)
    {
        return LLVMValueRef.CreateConstReal(LLVMTypeRef.Int32, Value);
    }
}
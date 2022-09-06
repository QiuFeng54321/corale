using LLVMSharp.Interop;

namespace PseudoCode.Core.CodeGen;

public class Integer : Expression
{
    public int Value;

    public override LLVMValueRef CodeGen()
    {
        return LLVMValueRef.CreateConstReal(LLVMTypeRef.Int32, Value);
    }
}
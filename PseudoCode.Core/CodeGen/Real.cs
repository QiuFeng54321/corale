using LLVMSharp.Interop;

namespace PseudoCode.Core.CodeGen;

public class Real : Expression
{
    public double Value;

    public override LLVMValueRef CodeGen()
    {
        return LLVMValueRef.CreateConstReal(LLVMTypeRef.Double, Value);
    }
}
using LLVMSharp.Interop;

namespace PseudoCode.Core.CodeGen;

public class String : Expression
{
    public override LLVMValueRef CodeGen()
    {
        return LLVM.BuildGlobalStringPtr(LLVM.)
    }
}
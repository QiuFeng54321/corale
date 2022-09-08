using LLVMSharp.Interop;
using PseudoCode.Core.Parsing;

namespace PseudoCode.Core.CodeGen;

public class Real : Expression
{
    public double Value;

    public override LLVMValueRef CodeGen(CodeGenContext ctx)
    {
        return LLVMValueRef.CreateConstReal(LLVMTypeRef.Double, Value);
    }
}
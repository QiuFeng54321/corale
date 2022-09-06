using LLVMSharp.Interop;

namespace PseudoCode.Core.CodeGen;

public abstract class Expression : AstNode
{
    public abstract LLVMValueRef CodeGen();
}
using LLVMSharp.Interop;
using PseudoCode.Core.Parsing;

namespace PseudoCode.Core.CodeGen;

public abstract class Expression : AstNode
{
    public abstract LLVMValueRef CodeGen(CodeGenContext ctx);
}
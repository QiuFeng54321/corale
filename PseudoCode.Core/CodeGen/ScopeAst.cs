using LLVMSharp.Interop;

namespace PseudoCode.Core.CodeGen;

public class ScopeAst : AstNode
{
    public List<Statement> Statements = new();
    public Dictionary<string, LLVMTypeRef> Types = new();
    public Dictionary<string, LLVMValueRef> Variables = new();
}
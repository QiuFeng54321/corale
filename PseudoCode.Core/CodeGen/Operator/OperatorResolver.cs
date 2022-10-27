using LLVMSharp.Interop;
using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.CodeGen.Operator;
using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.CodeGen;

public abstract class OperatorResolver
{
    public OperatorResolverMap ResolverMap;

    public abstract Symbol Resolve(Symbol left, Symbol right, PseudoOperator op, CodeGenContext ctx,
        CompilationUnit cu);

    private protected static Instruction MakeInst(IntCmpInstruction instruction, LLVMIntPredicate pred)
    {
        return (lhs, rhs, name) => instruction(pred, lhs, rhs, name);
    }

    private protected static Instruction MakeInst(RealCmpInstruction instruction, LLVMRealPredicate pred)
    {
        return (lhs, rhs, name) => instruction(pred, lhs, rhs, name);
    }

    private protected delegate LLVMValueRef Instruction(LLVMValueRef lhs, LLVMValueRef rhs, string name = "");

    private protected delegate LLVMValueRef IntCmpInstruction(LLVMIntPredicate pred, LLVMValueRef lhs, LLVMValueRef rhs,
        string name = "");

    private protected delegate LLVMValueRef RealCmpInstruction(LLVMRealPredicate pred, LLVMValueRef lhs,
        LLVMValueRef rhs,
        string name = "");
}
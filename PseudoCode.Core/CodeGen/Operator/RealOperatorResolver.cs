using LLVMSharp.Interop;
using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.CodeGen.Operator;

public class RealOperatorResolver : OperatorResolver
{
    public override Symbol Resolve(Symbol left, Symbol right, PseudoOperator op, CodeGenContext ctx, CompilationUnit cu)
    {
        var leftLLVMValue = left.GetRealValueRef(ctx, cu);
        var resType = BuiltinTypes.Real.Type;
        var tempName = ReservedNames.Real;
        LLVMValueRef res;
        // Unary
        if (right == null)
        {
            if (op != PseudoOperator.Negative) return null;
            res = cu.Builder.BuildFNeg(leftLLVMValue, resType.Kind.RequestTemp(ctx));
            return Symbol.MakeTemp(resType, res);
        }

        var rightLLVMValue = right.GetRealValueRef(ctx, cu);

        if (right.Type.Kind == Types.Integer)
            rightLLVMValue =
                cu.Builder.BuildSIToFP(rightLLVMValue, BuiltinTypes.Real.Type, resType.Kind.RequestTemp(ctx));
        if (right.Type.Kind is not (Types.Integer or Types.Real)) return null;

        Instruction func;
        switch (op)
        {
            case PseudoOperator.Add:
                func = cu.Builder.BuildFAdd;
                break;
            case PseudoOperator.Subtract:
                func = cu.Builder.BuildFSub;
                break;
            case PseudoOperator.Multiply:
                func = cu.Builder.BuildFMul;
                break;
            case PseudoOperator.Divide or PseudoOperator.IntDivide:
                func = cu.Builder.BuildFDiv;
                break;
            case PseudoOperator.Equal:
                func = MakeInst(cu.Builder.BuildFCmp, LLVMRealPredicate.LLVMRealOEQ);
                break;
            case PseudoOperator.GreaterEqual:
                func = MakeInst(cu.Builder.BuildFCmp, LLVMRealPredicate.LLVMRealOGE);
                break;
            case PseudoOperator.SmallerEqual:
                func = MakeInst(cu.Builder.BuildFCmp, LLVMRealPredicate.LLVMRealOLE);
                break;
            case PseudoOperator.Greater:
                func = MakeInst(cu.Builder.BuildFCmp, LLVMRealPredicate.LLVMRealOGT);
                break;
            case PseudoOperator.Smaller:
                func = MakeInst(cu.Builder.BuildFCmp, LLVMRealPredicate.LLVMRealOLT);
                break;
            default:
                return null;
        }

        if (op.IsComparison())
        {
            // res = cu.Builder.BuildIntCast(res, LLVMTypeRef.Int1, ctx.NameGenerator.Request(ReservedNames.Temp));
            resType = BuiltinTypes.Boolean.Type;
            tempName = ReservedNames.Boolean;
        }

        res = func(leftLLVMValue, rightLLVMValue, ctx.NameGenerator.RequestTemp(tempName));
        if (op == PseudoOperator.IntDivide)
        {
            resType = BuiltinTypes.Integer.Type;
            res = cu.Builder.BuildFPToSI(res, BuiltinTypes.Integer.Type.GetLLVMType(), resType.Kind.RequestTemp(ctx));
        }

        return Symbol.MakeTemp(resType, res);
    }
}
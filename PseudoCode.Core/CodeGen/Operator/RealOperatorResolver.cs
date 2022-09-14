using LLVMSharp.Interop;
using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.CodeGen.Operator;

public class RealOperatorResolver : OperatorResolver
{
    public override Symbol Resolve(Symbol left, Symbol right, PseudoOperator op, CodeGenContext ctx)
    {
        var leftLLVMValue = left.GetRealValue(ctx).ValueRef;
        var resType = BuiltinTypes.Real.Type;
        var tempName = ReservedNames.Real;
        LLVMValueRef res;
        // Unary
        if (right == null)
        {
            if (op != PseudoOperator.Negative) return null;
            res = ctx.Builder.BuildFNeg(leftLLVMValue, ctx.NameGenerator.RequestTemp(ReservedNames.Real));
            return Symbol.MakeTemp(resType, res);
        }

        var rightLLVMValue = right.GetRealValue(ctx).ValueRef;

        if (right.Type.Kind == Types.Integer)
            rightLLVMValue = ctx.Builder.BuildSIToFP(rightLLVMValue, BuiltinTypes.Real.Type,
                ctx.NameGenerator.RequestTemp(ReservedNames.Real));
        if (right.Type.Kind is not (Types.Integer or Types.Real)) return null;

        Instruction func;
        switch (op)
        {
            case PseudoOperator.Add:
                func = ctx.Builder.BuildFAdd;
                break;
            case PseudoOperator.Subtract:
                func = ctx.Builder.BuildFSub;
                break;
            case PseudoOperator.Multiply:
                func = ctx.Builder.BuildFMul;
                break;
            case PseudoOperator.Divide or PseudoOperator.IntDivide:
                func = ctx.Builder.BuildFDiv;
                break;
            case PseudoOperator.Equal:
                func = MakeInst(ctx.Builder.BuildFCmp, LLVMRealPredicate.LLVMRealOEQ);
                break;
            case PseudoOperator.GreaterEqual:
                func = MakeInst(ctx.Builder.BuildFCmp, LLVMRealPredicate.LLVMRealOGE);
                break;
            case PseudoOperator.SmallerEqual:
                func = MakeInst(ctx.Builder.BuildFCmp, LLVMRealPredicate.LLVMRealOLE);
                break;
            case PseudoOperator.Greater:
                func = MakeInst(ctx.Builder.BuildFCmp, LLVMRealPredicate.LLVMRealOGT);
                break;
            case PseudoOperator.Smaller:
                func = MakeInst(ctx.Builder.BuildFCmp, LLVMRealPredicate.LLVMRealOLT);
                break;
            default:
                return null;
        }

        if (op.IsComparison())
        {
            // res = ctx.Builder.BuildIntCast(res, LLVMTypeRef.Int1, ctx.NameGenerator.Request(ReservedNames.Temp));
            resType = BuiltinTypes.Boolean.Type;
            tempName = ReservedNames.Boolean;
        }

        res = func(leftLLVMValue, rightLLVMValue, ctx.NameGenerator.RequestTemp(tempName));
        if (op == PseudoOperator.IntDivide)
            res = ctx.Builder.BuildFPToSI(res, BuiltinTypes.Integer.Type.GetLLVMType(),
                ctx.NameGenerator.RequestTemp(ReservedNames.Integer));

        return Symbol.MakeTemp(resType, res);
    }
}
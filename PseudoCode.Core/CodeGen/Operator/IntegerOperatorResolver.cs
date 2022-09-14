using LLVMSharp.Interop;
using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.CodeGen.Operator;

public class IntegerOperatorResolver : OperatorResolver
{
    public override Symbol Resolve(Symbol left, Symbol right, PseudoOperator op, CodeGenContext ctx)
    {
        var leftLLVMValue = left.GetRealValue(ctx).ValueRef;
        var resType = BuiltinTypes.Integer.Type;
        LLVMValueRef res;
        // Unary
        var tempName = ReservedNames.Integer;
        if (right == null)
        {
            if (op != PseudoOperator.Negative) return null;
            res = ctx.Builder.BuildNeg(leftLLVMValue, ctx.NameGenerator.RequestTemp(tempName));
            return Symbol.MakeTemp(resType, res);
        }

        // Right is real => Cast left to real => Call resolve on real
        if (right.Type.Kind is Types.Real)
        {
            leftLLVMValue = ctx.Builder.BuildSIToFP(leftLLVMValue, BuiltinTypes.Real.Type,
                ctx.NameGenerator.RequestTemp(ReservedNames.Real));
            var castedLeft = Symbol.MakeTemp(BuiltinTypes.Real.Type, leftLLVMValue);
            return ResolverMap.Resolve(castedLeft, right, op, ctx);
        }

        var rightLLVMValue = right.GetRealValue(ctx).ValueRef;

        Instruction func;
        switch (op)
        {
            case PseudoOperator.Add:
                func = ctx.Builder.BuildAdd;
                break;
            case PseudoOperator.Subtract:
                func = ctx.Builder.BuildSub;
                break;
            case PseudoOperator.Multiply:
                func = ctx.Builder.BuildMul;
                break;
            case PseudoOperator.Divide:
                func = ctx.Builder.BuildFDiv;
                leftLLVMValue = ctx.Builder.BuildSIToFP(leftLLVMValue, BuiltinTypes.Real.Type.GetLLVMType(),
                    ctx.NameGenerator.RequestTemp(ReservedNames.Real));
                rightLLVMValue = ctx.Builder.BuildSIToFP(rightLLVMValue, BuiltinTypes.Real.Type.GetLLVMType(),
                    ctx.NameGenerator.RequestTemp(ReservedNames.Real));
                resType = BuiltinTypes.Real.Type;
                break;
            case PseudoOperator.IntDivide:
                func = ctx.Builder.BuildSDiv;
                break;
            case PseudoOperator.Mod:
                func = ctx.Builder.BuildSRem;
                break;
            case PseudoOperator.Equal:
                func = MakeInst(ctx.Builder.BuildICmp, LLVMIntPredicate.LLVMIntEQ);
                break;
            case PseudoOperator.GreaterEqual:
                func = MakeInst(ctx.Builder.BuildICmp, LLVMIntPredicate.LLVMIntSGE);
                break;
            case PseudoOperator.SmallerEqual:
                func = MakeInst(ctx.Builder.BuildICmp, LLVMIntPredicate.LLVMIntSLE);
                break;
            case PseudoOperator.Greater:
                func = MakeInst(ctx.Builder.BuildICmp, LLVMIntPredicate.LLVMIntSGT);
                break;
            case PseudoOperator.Smaller:
                func = MakeInst(ctx.Builder.BuildICmp, LLVMIntPredicate.LLVMIntSLT);
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

        return Symbol.MakeTemp(resType, res);
    }
}
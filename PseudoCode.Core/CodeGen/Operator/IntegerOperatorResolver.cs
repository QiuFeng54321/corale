using LLVMSharp.Interop;
using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.CodeGen.Operator;

public class IntegerOperatorResolver : OperatorResolver
{
    public override Symbol Resolve(Symbol left, Symbol right, PseudoOperator op, CodeGenContext ctx)
    {
        var leftLLVMValue = left.GetRealValue(ctx).ValueRef;
        var resType = BuiltinTypes.Integer.Type;
        LLVMValueRef res;
        if (right == null)
        {
            if (op != PseudoOperator.Negative) return null;
            res = ctx.Builder.BuildNeg(leftLLVMValue, resType.Kind.RequestTemp(ctx));
            return Symbol.MakeTemp(resType, res);
        }

        // Right is real => Cast left to real => Call resolve on real
        if (right.Type.Kind is Types.Real)
        {
            resType = BuiltinTypes.Real.Type;
            leftLLVMValue =
                ctx.Builder.BuildSIToFP(leftLLVMValue, BuiltinTypes.Real.Type, resType.Kind.RequestTemp(ctx));
            var castedLeft = Symbol.MakeTemp(resType, leftLLVMValue);
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
                resType = BuiltinTypes.Real.Type;
                leftLLVMValue = ctx.Builder.BuildSIToFP(leftLLVMValue, BuiltinTypes.Real.Type.GetLLVMType(),
                    resType.Kind.RequestTemp(ctx));
                rightLLVMValue = ctx.Builder.BuildSIToFP(rightLLVMValue, BuiltinTypes.Real.Type.GetLLVMType(),
                    resType.Kind.RequestTemp(ctx));
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
        }

        res = func(leftLLVMValue, rightLLVMValue, resType.Kind.RequestTemp(ctx));

        return Symbol.MakeTemp(resType, res);
    }
}
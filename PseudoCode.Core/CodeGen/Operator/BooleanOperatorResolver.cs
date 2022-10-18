using LLVMSharp.Interop;
using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.CodeGen.Operator;

public class BooleanOperatorResolver : OperatorResolver
{
    public override Symbol Resolve(Symbol left, Symbol right, PseudoOperator op, CodeGenContext ctx)
    {
        var leftLLVMValue = left.GetRealValue(ctx).ValueRef;
        var resType = BuiltinTypes.Boolean.Type;
        LLVMValueRef res;
        // Unary
        if (right == null)
        {
            if (op != PseudoOperator.Not) return null;
            res = ctx.Builder.BuildNot(leftLLVMValue, resType.Kind.RequestTemp(ctx));
            return Symbol.MakeTemp(resType, res);
        }

        var rightLLVMValue = right.GetRealValue(ctx).ValueRef;
        if (right.Type.Kind is not Types.Boolean) return null;
        Instruction func;
        switch (op)
        {
            case PseudoOperator.And:
                func = ctx.Builder.BuildAnd;
                break;
            case PseudoOperator.Or:
                func = ctx.Builder.BuildOr;
                break;
            case PseudoOperator.Equal:
                func = MakeInst(ctx.Builder.BuildICmp, LLVMIntPredicate.LLVMIntEQ);
                break;
            default:
                return null;
        }

        res = func(leftLLVMValue, rightLLVMValue, resType.Kind.RequestTemp(ctx));

        return Symbol.MakeTemp(resType, res);
    }
}
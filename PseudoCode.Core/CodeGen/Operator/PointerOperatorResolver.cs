using LLVMSharp.Interop;
using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.CodeGen.Operator;

public class PointerOperatorResolver : OperatorResolver
{
    public override Symbol Resolve(Symbol left, Symbol right, PseudoOperator op, CodeGenContext ctx, CompilationUnit cu)
    {
        var resType = left.Type;
        if (right == null)
        {
            if (op == PseudoOperator.GetPointed)
            {
                resType = left.Type.ElementType;
                var val = left.GetRealValueRef(ctx, cu);
                var resSym = Symbol.MakeTemp(resType, val, true);
                return resSym;
            }

            throw new InvalidOperationException();
        }

        var leftLLVMValue = left.GetRealValueRef(ctx, cu);
        var rightLLVMValue = right.GetRealValueRef(ctx, cu);
        LLVMValueRef res = null;
        if (right.Type.Kind is Types.Integer)
        {
            res = cu.Builder.BuildPtrToInt(leftLLVMValue, LLVMTypeRef.Int64, Types.Integer.RequestTemp(ctx));
            rightLLVMValue =
                cu.Builder.BuildIntCast(rightLLVMValue, LLVMTypeRef.Int64, Types.Integer.RequestTemp(ctx));
            if (op is PseudoOperator.Add)
                res = cu.Builder.BuildAdd(res, rightLLVMValue, Types.Integer.RequestTemp(ctx));
            else if (op is PseudoOperator.Subtract)
                res = cu.Builder.BuildSub(res, rightLLVMValue, Types.Integer.RequestTemp(ctx));
            res = cu.Builder.BuildIntToPtr(res, left.Type.GetLLVMType(), left.Type.Kind.RequestTemp(ctx));
        }
        else if (right.Type.Kind is Types.Pointer && op is PseudoOperator.Subtract)
        {
            if (!Equals(right.Type.ElementType, left.Type.ElementType)) throw new InvalidOperationException();
            res = cu.Builder.BuildPtrDiff2(left.Type.ElementType.GetLLVMType(), leftLLVMValue, rightLLVMValue,
                left.Type.Kind.RequestTemp(ctx));
            resType = BuiltinTypes.Integer.Type;
        }

        if (op.IsComparison())
        {
            Instruction func;
            switch (op)
            {
                case PseudoOperator.Equal:
                    func = MakeInst(cu.Builder.BuildICmp, LLVMIntPredicate.LLVMIntEQ);
                    break;
                case PseudoOperator.GreaterEqual:
                    func = MakeInst(cu.Builder.BuildICmp, LLVMIntPredicate.LLVMIntSGE);
                    break;
                case PseudoOperator.SmallerEqual:
                    func = MakeInst(cu.Builder.BuildICmp, LLVMIntPredicate.LLVMIntSLE);
                    break;
                case PseudoOperator.Greater:
                    func = MakeInst(cu.Builder.BuildICmp, LLVMIntPredicate.LLVMIntSGT);
                    break;
                case PseudoOperator.Smaller:
                    func = MakeInst(cu.Builder.BuildICmp, LLVMIntPredicate.LLVMIntSLT);
                    break;
                default:
                    return null;
            }

            // res = cu.Builder.BuildIntCast(res, LLVMTypeRef.Int1, ctx.NameGenerator.Request(ReservedNames.Temp));
            resType = BuiltinTypes.Boolean.Type;
            res = func(leftLLVMValue, rightLLVMValue, resType.Kind.RequestTemp(ctx));
        }


        return Symbol.MakeTemp(resType, res);
    }
}
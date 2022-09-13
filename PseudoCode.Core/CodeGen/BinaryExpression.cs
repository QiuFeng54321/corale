using LLVMSharp.Interop;
using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.CodeGen;

public class BinaryExpression : Expression
{
    public Expression Left, Right;
    public PseudoOperator Operator;

    public override Symbol CodeGen(CodeGenContext ctx, Block block)
    {
        var left = Left.CodeGen(ctx, block);
        var right = Right.CodeGen(ctx, block);
        // TODO: Insert Custom Operators here

        var leftLLVMValue = left.GetRealValue(ctx).ValueRef;
        var rightLLVMValue = right.GetRealValue(ctx).ValueRef;
        if (left.Type.Equals(BuiltinTypes.Integer.Type) && right.Type.Equals(BuiltinTypes.Integer.Type))
        {
            Instruction func = Operator switch
            {
                PseudoOperator.Add => ctx.Builder.BuildAdd,
                PseudoOperator.Subtract => ctx.Builder.BuildSub,
                PseudoOperator.Multiply => ctx.Builder.BuildMul,
                PseudoOperator.Divide => ctx.Builder.BuildFDiv,
                PseudoOperator.IntDivide => ctx.Builder.BuildSDiv,
                PseudoOperator.Mod => ctx.Builder.BuildSRem,
                _ => throw new NotImplementedException()
            };
            if (Operator == PseudoOperator.Divide)
            {
                leftLLVMValue = ctx.Builder.BuildSIToFP(leftLLVMValue, BuiltinTypes.Real.Type.GetLLVMType(),
                    ctx.NameGenerator.Request(ReservedNames.Temp));
                rightLLVMValue = ctx.Builder.BuildSIToFP(rightLLVMValue, BuiltinTypes.Real.Type.GetLLVMType(),
                    ctx.NameGenerator.Request(ReservedNames.Temp));
            }

            var res = func(leftLLVMValue, rightLLVMValue, ctx.NameGenerator.Request(ReservedNames.Temp));
            return Symbol.MakeTemp(ReservedNames.Integer, BuiltinTypes.Integer.Type, ctx, res);
        }

        if (Equals(left.Type, BuiltinTypes.Real.Type) || Equals(right.Type, BuiltinTypes.Real.Type))
        {
            if (left.Type.Equals(BuiltinTypes.Integer.Type))
                leftLLVMValue = ctx.Builder.BuildSIToFP(leftLLVMValue, BuiltinTypes.Real.Type.GetLLVMType(),
                    ctx.NameGenerator.Request(ReservedNames.Temp));
            if (right.Type.Equals(BuiltinTypes.Integer.Type))
                rightLLVMValue = ctx.Builder.BuildSIToFP(rightLLVMValue, BuiltinTypes.Real.Type.GetLLVMType(),
                    ctx.NameGenerator.Request(ReservedNames.Temp));

            Instruction func = Operator switch
            {
                PseudoOperator.Add => ctx.Builder.BuildFAdd,
                PseudoOperator.Subtract => ctx.Builder.BuildFSub,
                PseudoOperator.Multiply => ctx.Builder.BuildFMul,
                PseudoOperator.Divide or PseudoOperator.IntDivide => ctx.Builder.BuildFDiv,
                _ => throw new NotImplementedException()
            };
            var res = func(leftLLVMValue, rightLLVMValue, ctx.NameGenerator.Request(ReservedNames.Temp));
            if (Operator == PseudoOperator.IntDivide)
                res = ctx.Builder.BuildFPToSI(res, BuiltinTypes.Integer.Type.GetLLVMType(),
                    ctx.NameGenerator.Request(ReservedNames.Temp));

            return Symbol.MakeTemp(ReservedNames.Real, BuiltinTypes.Real.Type, ctx, res);
        }

        throw new NotImplementedException();
    }

    private delegate LLVMValueRef Instruction(LLVMValueRef LHS, LLVMValueRef RHS, string Name = "");
}
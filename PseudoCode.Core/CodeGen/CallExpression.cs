using PseudoCode.Core.CodeGen.Containers;

namespace PseudoCode.Core.CodeGen;

public class CallExpression : Expression
{
    public List<Expression> Arguments;
    public Expression Function;

    public override Symbol CodeGen(CodeGenContext ctx, Block block)
    {
        var function = Function.CodeGen(ctx, block);
        var arguments = Arguments.Select(a => a.CodeGen(ctx, block)).ToArray();
        var overload = function.FindFunctionOverload(arguments.ToList());
        var retType = overload.Type.ReturnType;
        var ret = ctx.Builder.BuildCall2(overload.Type.GetLLVMType(), overload.ValueRef,
            arguments.Select(a => a.GetRealValueRef(ctx)).ToArray(),
            retType.Kind == Types.None ? "" : retType.Kind.RequestTemp(ctx));
        return retType.Kind == Types.None ? null : Symbol.MakeTemp(retType, ret);
    }

    public override string ToFormatString()
    {
        return $"{Function.ToFormatString()}({string.Join(", ", Arguments.Select(a => a.ToFormatString()))})";
    }
}
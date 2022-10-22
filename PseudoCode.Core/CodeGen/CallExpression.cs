using LLVMSharp.Interop;
using PseudoCode.Core.CodeGen.Containers;

namespace PseudoCode.Core.CodeGen;

public class CallExpression : Expression
{
    public List<Expression> Arguments;
    public Expression Function;

    public override Symbol CodeGen(CodeGenContext ctx, Function function1)
    {
        var function = Function.CodeGen(ctx, function1);
        var arguments = Arguments.Select(a => a.CodeGen(ctx, function1)).ToArray();
        var overload = function.FindFunctionOverload(arguments.ToList());
        var retType = overload.Type.ReturnType;
        var llvmArguments = new List<LLVMValueRef>();
        for (var index = 0; index < arguments.Length; index++)
        {
            var argValue = arguments[index];
            var funcArg = overload.Type.Arguments[index];
            llvmArguments.Add(funcArg.DefinitionAttribute.HasFlag(DefinitionAttribute.Reference)
                ? argValue.MemoryPointer
                : argValue.GetRealValueRef(ctx));
        }

        var ret = ctx.Builder.BuildCall2(overload.Type.GetLLVMType(), overload.ValueRef,
            llvmArguments.ToArray(),
            retType.Kind == Types.None ? "" : retType.Kind.RequestTemp(ctx));
        return retType.Kind == Types.None ? null : Symbol.MakeTemp(retType, ret);
    }

    public override string ToFormatString()
    {
        return $"{Function.ToFormatString()}({string.Join(", ", Arguments.Select(a => a.ToFormatString()))})";
    }
}
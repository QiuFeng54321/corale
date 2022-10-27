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
        return CodeGenCallFuncGroup(ctx, function, arguments);
    }

    public static Symbol CodeGenCallFuncGroup(CodeGenContext ctx, Symbol functionGroup, Symbol[] arguments)
    {
        var overload = functionGroup.FindFunctionOverload(arguments.ToList());
        return CodeGenCallFunc(ctx, overload, arguments);
    }

    public static Symbol CodeGenCallFunc(CodeGenContext ctx, Symbol overload, Symbol[] arguments)
    {
        var retType = overload.Type.ReturnType;
        var llvmArguments = new List<LLVMValueRef>();
        for (var index = 0; index < arguments.Length; index++)
        {
            var argValue = arguments[index];
            var funcArg = overload.Type.Arguments[index];
            llvmArguments.Add(funcArg.DefinitionAttribute.HasFlag(DefinitionAttribute.Reference)
                ? argValue.GetPointerValueRef()
                : argValue.GetRealValueRef(ctx));
        }

        var ret = ctx.Builder.BuildCall2(overload.Type.GetLLVMType(), overload.ValueRef,
            llvmArguments.ToArray(),
            retType.Type.Kind == Types.None ? "" : retType.Type.Kind.RequestTemp(ctx));
        return retType.Type.Kind == Types.None
            ? null
            : Symbol.MakeTemp(retType.Type, ret, retType.DefinitionAttribute.HasFlag(DefinitionAttribute.Reference));
    }

    public override string ToFormatString()
    {
        return $"{Function.ToFormatString()}({string.Join(", ", Arguments.Select(a => a.ToFormatString()))})";
    }
}
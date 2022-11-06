using LLVMSharp.Interop;
using PseudoCode.Core.Analyzing;
using PseudoCode.Core.CodeGen.Containers;

namespace PseudoCode.Core.CodeGen.Expressions;

public class CallExpression : Expression
{
    public List<Expression> Arguments;
    public Expression Function;

    public override Symbol CodeGen(CodeGenContext ctx, CompilationUnit cu, Function function1)
    {
        var function = Function.CodeGen(ctx, cu, function1);
        var arguments = Arguments.Select(a => a.CodeGen(ctx, cu, function1)).ToArray();
        if (function.IsError)
        {
            ctx.Analysis.Feedbacks.Add(new Feedback
            {
                Message = $"Function not found: {Function}",
                DebugInformation = DebugInformation,
                Severity = Feedback.SeverityType.Error
            });
            return DebugInformation.MakeErrorSymbol();
        }

        return CodeGenCallFuncGroup(ctx, cu, function, arguments);
    }

    public static Symbol CodeGenCallFuncGroup(CodeGenContext ctx, CompilationUnit cu, Symbol functionGroup,
        Symbol[] arguments)
    {
        var overload = functionGroup.FindFunctionOverload(arguments.ToList());
        if (overload == null)
        {
            ctx.Analysis.Feedbacks.Add(new Feedback
            {
                Message = $"No overload found for {functionGroup.Name}",
                Severity = Feedback.SeverityType.Error,
                DebugInformation = functionGroup.DebugInformation
            });
            return functionGroup.DebugInformation.MakeErrorSymbol();
        }

        return CodeGenCallFunc(ctx, cu, overload, arguments);
    }

    public static Symbol CodeGenCallFunc(CodeGenContext ctx, CompilationUnit cu, Symbol overload, Symbol[] arguments)
    {
        var retType = overload.Type.ReturnType;
        var llvmArguments = new List<LLVMValueRef>();
        for (var index = 0; index < arguments.Length; index++)
        {
            var argValue = arguments[index];
            var funcArg = overload.Type.Arguments[index];
            llvmArguments.Add(funcArg.DefinitionAttribute.HasFlag(DefinitionAttribute.Reference)
                ? argValue.GetPointerValueRef(ctx)
                : argValue.GetRealValueRef(ctx, cu));
        }

        var ret = cu.Builder.BuildCall2(overload.Type.GetLLVMType(), overload.GetRealValueRef(ctx, cu),
            llvmArguments.ToArray(),
            retType.Type.Kind == Types.None ? "" : retType.Type.Kind.RequestTemp(ctx));
        return retType.Type.Kind == Types.None
            ? Symbol.MakeErrorSymbol(null)
            : Symbol.MakeTemp(retType.Type, ret, retType.DefinitionAttribute.HasFlag(DefinitionAttribute.Reference));
    }

    public override string ToFormatString()
    {
        return $"{Function.ToFormatString()}({string.Join(", ", Arguments.Select(a => a.ToFormatString()))})";
    }
}
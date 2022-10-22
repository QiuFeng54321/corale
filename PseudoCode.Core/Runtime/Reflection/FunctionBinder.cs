using System.Reflection;
using System.Runtime.InteropServices;
using PseudoCode.Core.CodeGen;
using PseudoCode.Core.CodeGen.Containers;
using Type = System.Type;

namespace PseudoCode.Core.Runtime.Reflection;

public static class FunctionBinder
{
    public static void MakeFromType(CodeGenContext ctx, Type type)
    {
        foreach (var method in type.GetMethods()) MakeDefinitionOfNativeMethod(ctx, method);
    }

    public static bool MakeDefinitionOfNativeMethod(CodeGenContext ctx, MethodInfo methodInfo)
    {
        if (methodInfo.GetCustomAttributes(typeof(BuiltinNativeFunctionAttribute), true).Length == 0
            || methodInfo.GetCustomAttributes(typeof(UnmanagedCallersOnlyAttribute), true).Length == 0)
            return false;

        var functionName = GetFunctionName(methodInfo);
        var paramList = GetNativeMethodParamList(ctx, methodInfo);
        var returnDef = GetNativeMethodReturnDefinition(ctx, methodInfo);
        var function = ctx.CompilationUnit.MakeFunction(functionName, paramList, returnDef, true);
        function.GeneratePrototype(ctx);
        var functionPointer = methodInfo.MethodHandle.GetFunctionPointer();
        function.LinkToFunctionPointer(ctx, functionPointer);

        return true;
    }

    private static string GetFunctionName(MethodInfo methodInfo)
    {
        return ReflectionHelper.GetAttribute<BuiltinNativeFunctionAttribute>(methodInfo)?.Name ?? methodInfo.Name;
    }

    private static List<Symbol> GetNativeMethodParamList(CodeGenContext ctx, MethodInfo methodInfo)
    {
        return methodInfo.GetParameters().Select(info =>
                new Symbol(info.Name, false, TypeBinder.GetTypeSymbolFromSystemType(ctx, info.ParameterType).Type))
            .ToList();
    }

    private static Symbol GetNativeMethodReturnDefinition(CodeGenContext ctx, MethodInfo methodInfo)
    {
        var typeDescriptor = TypeBinder.GetTypeSymbolFromSystemType(ctx, methodInfo.ReturnType);
        return typeDescriptor;
    }
}
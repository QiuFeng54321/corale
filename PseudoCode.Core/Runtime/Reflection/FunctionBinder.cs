using System.Reflection;
using System.Runtime.InteropServices;
using PseudoCode.Core.CodeGen;
using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.Runtime.Errors;
using Type = System.Type;

namespace PseudoCode.Core.Runtime.Reflection;

public class FunctionBinder
{
    public static readonly Dictionary<Type, Symbol> TypeMap = new()
    {
        [typeof(int)] = BuiltinTypes.Integer,
        [typeof(double)] = BuiltinTypes.Real,
        [typeof(string)] = BuiltinTypes.CharPtr,
        [typeof(char)] = BuiltinTypes.Char,
        [typeof(byte)] = BuiltinTypes.Char,
        [typeof(BlittableChar)] = BuiltinTypes.Char,
        [typeof(bool)] = BuiltinTypes.Boolean,
        [typeof(BlittableBoolean)] = BuiltinTypes.Boolean,
        // [typeof(DateOnly)] = BuiltinTypes.,
        [typeof(void)] = BuiltinTypes.Void
    };

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
        var paramList = GetNativeMethodParamList(methodInfo);
        var returnDef = GetNativeMethodReturnDefinition(methodInfo);
        var function = ctx.CompilationUnit.MakeFunction(functionName, paramList, returnDef, true);
        function.GeneratePrototype(ctx);
        var functionPointer = methodInfo.MethodHandle.GetFunctionPointer();
        function.LinkToFunctionPointer(ctx, functionPointer);

        return true;
    }

    private static string GetFunctionName(MethodInfo methodInfo)
    {
        if (methodInfo.GetCustomAttributes(typeof(BuiltinNativeFunctionAttribute)) is BuiltinNativeFunctionAttribute[]
                nativeNameAttrs &&
            nativeNameAttrs.Length != 0)
            return nativeNameAttrs[0].Name;

        return methodInfo.Name;
    }

    private static List<Symbol> GetNativeMethodParamList(MethodInfo methodInfo)
    {
        return methodInfo.GetParameters().Select(info =>
            new Symbol(info.Name, false, GetTypeDescriptorFromSystemType(info.ParameterType).Type)).ToList();
    }

    private static Symbol GetTypeDescriptorFromSystemType(Type infoParameterType)
    {
        if (TypeMap.ContainsKey(infoParameterType)) return TypeMap[infoParameterType];
        if (infoParameterType.IsPointer)
            return GetTypeDescriptorFromSystemType(infoParameterType.GetElementType()).MakePointerType();

        if (infoParameterType.IsValueType)
        {
        }

        throw new InvalidTypeError(infoParameterType.ToString());
    }

    private static Symbol GetNativeMethodReturnDefinition(MethodInfo methodInfo)
    {
        var typeDescriptor = GetTypeDescriptorFromSystemType(methodInfo.ReturnType);
        return typeDescriptor;
    }
}
using System.Reflection;
using System.Runtime.InteropServices;
using LLVMSharp.Interop;
using PseudoCode.Core.CodeGen;
using PseudoCode.Core.CodeGen.Containers;
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

    public static void MakeFromType(CodeGenContext ctx, Block block, Type type)
    {
        foreach (var method in type.GetMethods()) MakeDefinitionOfNativeMethod(ctx, block, method);
    }

    public static bool MakeDefinitionOfNativeMethod(CodeGenContext ctx, Block block, MethodInfo methodInfo)
    {
        if (methodInfo.GetCustomAttributes(typeof(BuiltinNativeFunctionAttribute), true).Length == 0
            || methodInfo.GetCustomAttributes(typeof(UnmanagedCallersOnlyAttribute), true).Length == 0)
            return false;

        var functionName = GetFunctionName(methodInfo);
        // unsafe
        // {
        //     delegate* unmanaged[Cdecl] <char, char> a = &BuiltinFunctions.LowerCase;
        //     methodInfo.MethodHandle.Value
        // }

        var paramList = GetNativeMethodParamList(methodInfo);
        var returnDef = GetNativeMethodReturnDefinition(methodInfo);
        var functionType =
            LLVMTypeRef.CreateFunction(returnDef.Type.GetLLVMType(),
                paramList.Select(p => p.Type.GetLLVMType()).ToArray());
        var function = ctx.Module.AddFunction(functionName, functionType);
        function.Linkage = LLVMLinkage.LLVMExternalLinkage;
        // var functionPointer = Marshal.GetFunctionPointerForDelegate(del);
        var functionPointer = methodInfo.MethodHandle.GetFunctionPointer();
        ctx.Engine.AddGlobalMapping(function, functionPointer);
        var pseudoFunctionType = new CodeGen.Type
        {
            Arguments = paramList,
            ReturnType = returnDef.Type,
            TypeName = CodeGen.Type.GenerateFunctionTypeName(paramList, returnDef.Type),
            Kind = CodeGen.Types.Function
        };
        pseudoFunctionType.SetLLVMType(functionType);
        var functionSymbol = new Symbol(functionName, false, pseudoFunctionType)
        {
            ValueRef = function
        };
        if (!block.Namespace.TryGetSymbol(functionName, out var functionOverloadsSymbol))
        {
            functionOverloadsSymbol = new Symbol(functionName, false, pseudoFunctionType);
            block.Namespace.AddSymbol(functionOverloadsSymbol);
        }

        functionOverloadsSymbol.FunctionOverloads.Add(functionSymbol);
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
        return TypeMap[infoParameterType];
    }

    private static Symbol GetNativeMethodReturnDefinition(MethodInfo methodInfo)
    {
        var typeDescriptor = GetTypeDescriptorFromSystemType(methodInfo.ReturnType);
        return typeDescriptor;
    }
}
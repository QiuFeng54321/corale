using System.Reflection;
using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Operations;
using PseudoCode.Core.Runtime.Types;
using PseudoCode.Core.Runtime.Types.Descriptor;
using Type = System.Type;

namespace PseudoCode.Core.Runtime.Reflection;

public class FunctionBinder
{
    public delegate Instance BuiltinFunction(Scope parentScope, PseudoProgram program, Instance[] arguments);

    public static readonly Dictionary<Type, string> TypeMap = new()
    {
        [typeof(int)] = "INTEGER",
        [typeof(RealNumberType)] = "REAL",
        [typeof(string)] = "STRING",
        [typeof(char)] = "CHAR",
        [typeof(bool)] = "BOOLEAN",
        [typeof(DateOnly)] = "DATE",
        [typeof(void)] = "NULL"
    };

    // Will be extended to support arrays
    public static ITypeDescriptor GetTypeDescriptorFromSystemType(Type type)
    {
        return new PlainTypeDescriptor(TypeMap[type]);
    }

    public static void AddBuiltinFunctionOperations(Type type, Scope parentScope, PseudoProgram program)
    {
        foreach (var (definition, func) in MakeDefinition(type, parentScope, program))
            parentScope.AddOperation(new MakeBuiltinFunctionOperation(parentScope, program)
            {
                Name = definition.Name,
                Definition = definition,
                Func = func,
                PoiLocation = SourceLocation.Identity,
                SourceRange = SourceRange.Identity
            });
    }

    public static IEnumerable<(Definition, BuiltinFunction)> MakeDefinition(Type type, Scope parentScope,
        PseudoProgram program)
    {
        var methods = type.GetMethods();
        foreach (var methodInfo in methods)
            if (MakeDefinitionOfMethod(parentScope, program, methodInfo, out var definition))
                yield return (definition, (BuiltinFunction)methodInfo.CreateDelegate(typeof(BuiltinFunction)));
            else if (MakeDefinitionOfNativeMethod(parentScope, program, methodInfo, out var nativeDefinition))
                yield return (nativeDefinition, (scope, pseudoProgram, arguments) =>
                {
                    var nativeDefinitionType = (BuiltinFunctionType)nativeDefinition.Type;
                    var res = methodInfo.Invoke(null,
                        arguments.Select(instance => instance.Get<object>()).ToArray());
                    return res == null ? Instance.Null : nativeDefinitionType.ReturnType.Type.Instance(res);
                });
    }

    private static bool MakeDefinitionOfMethod(Scope parentScope, PseudoProgram program, MethodInfo methodInfo,
        out Definition definition)
    {
        if (methodInfo.GetCustomAttributes(typeof(BuiltinFunctionAttribute), true).Length == 0)
        {
            definition = null;
            return false;
        }

        var functionName = GetFunctionName(methodInfo);
        var documentation = GetFunctionDocumentation(methodInfo);

        var paramList = GetMethodParamList(parentScope, program, methodInfo).ToArray();
        var returnDef = GetMethodReturnDefinition(parentScope, program, methodInfo);

        definition = new Definition(parentScope, program)
        {
            Name = functionName,
            References = new List<SourceRange>(),
            SourceRange = SourceRange.Identity,
            Type = new BuiltinFunctionType(parentScope, program)
            {
                ParameterInfos = paramList,
                ReturnType = returnDef
            },
            TypeDescriptor = new FunctionDescriptor(returnDef.TypeDescriptor, paramList),
            Documentation = documentation
        };
        return true;
    }

    private static bool MakeDefinitionOfNativeMethod(Scope parentScope, PseudoProgram program, MethodInfo methodInfo,
        out Definition definition)
    {
        if (methodInfo.GetCustomAttributes(typeof(BuiltinNativeFunctionAttribute), true).Length == 0)
        {
            definition = null;
            return false;
        }

        var functionName = GetFunctionName(methodInfo);
        var documentation = GetFunctionDocumentation(methodInfo);

        var paramList = GetNativeMethodParamList(parentScope, program, methodInfo).ToArray();
        var returnDef = GetNativeMethodReturnDefinition(parentScope, program, methodInfo);

        definition = new Definition(parentScope, program)
        {
            Name = functionName,
            References = new List<SourceRange>(),
            SourceRange = SourceRange.Identity,
            Type = new BuiltinFunctionType(parentScope, program)
            {
                ParameterInfos = paramList,
                ReturnType = returnDef
            },
            TypeDescriptor = new FunctionDescriptor(returnDef.TypeDescriptor, paramList),
            Documentation = documentation
        };
        return true;
    }

    private static string GetFunctionDocumentation(MethodInfo methodInfo)
    {
        if (methodInfo.GetCustomAttribute(typeof(DocumentationAttribute)) is DocumentationAttribute attribute)
        {
            return attribute.Documentation;
        }

        return "";
    }

    private static string GetFunctionName(MethodInfo methodInfo)
    {
        if (methodInfo.GetCustomAttributes(typeof(BuiltinFunctionAttribute)) is BuiltinFunctionAttribute[] nameAttrs &&
            nameAttrs.Length != 0)
            return nameAttrs[0].Name;

        if (methodInfo.GetCustomAttributes(typeof(BuiltinNativeFunctionAttribute)) is BuiltinNativeFunctionAttribute[]
                nativeNameAttrs &&
            nativeNameAttrs.Length != 0)
            return nativeNameAttrs[0].Name;

        return methodInfo.Name;
    }
    private static List<Definition> GetMethodParamList(Scope parentScope, PseudoProgram program, MethodInfo methodInfo)
    {
        return methodInfo.GetCustomAttributes(typeof(ParamTypeAttribute))
            .Cast<ParamTypeAttribute>()
            .Select(param => new Definition(parentScope, program)
            {
                Name = param.Name,
                Attributes =
                    param.IsReference ? Definition.Attribute.Reference : Definition.Attribute.Immutable,
                SourceRange = SourceRange.Identity,
                TypeDescriptor = param.MakeTypeDescriptor()
            }).ToList();
    }

    private static Definition GetMethodReturnDefinition(Scope parentScope, PseudoProgram program, MethodInfo methodInfo)
    {
        if (methodInfo.GetCustomAttributes(typeof(ReturnTypeAttribute)) is not ReturnTypeAttribute[]
                returnTypeAttributes || returnTypeAttributes.Length == 0) return null;
        var typeDescriptor = returnTypeAttributes[0].MakeTypeDescriptor();
        return new Definition(parentScope, program)
        {
            Name = typeDescriptor.ToString(),
            TypeDescriptor = typeDescriptor,
            SourceRange = SourceRange.Identity,
            Attributes = Definition.Attribute.Type | Definition.Attribute.Immutable
        };
    }

    private static List<Definition> GetNativeMethodParamList(Scope parentScope, PseudoProgram program,
        MethodInfo methodInfo)
    {
        return methodInfo.GetParameters().Select(info => new Definition(parentScope, program)
        {
            Name = info.Name,
            TypeDescriptor = GetTypeDescriptorFromSystemType(info.ParameterType),
            Attributes = Definition.Attribute.Immutable,
            SourceRange = SourceRange.Identity
        }).ToList();
    }

    private static Definition GetNativeMethodReturnDefinition(Scope parentScope, PseudoProgram program,
        MethodInfo methodInfo)
    {
        var typeDescriptor = GetTypeDescriptorFromSystemType(methodInfo.ReturnType);
        return new Definition(parentScope, program)
        {
            Name = typeDescriptor.ToString(),
            TypeDescriptor = typeDescriptor,
            SourceRange = SourceRange.Identity,
            Attributes = Definition.Attribute.Type | Definition.Attribute.Immutable
        };
    }
}
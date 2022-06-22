using System.Reflection;
using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Operations;
using PseudoCode.Core.Runtime.Types;
using Type = PseudoCode.Core.Runtime.Types.Type;

namespace PseudoCode.Core.Runtime.Reflection;

public class FunctionBinder
{
    public delegate Instance BuiltinFunction(Scope parentScope, PseudoProgram program, Instance[] arguments);

    public static void AddBuiltinFunctionOperations(System.Type type, Scope parentScope, PseudoProgram program)
    {
        foreach (var (definition, func) in MakeDefinition(type, parentScope, program))
        {
            parentScope.AddOperation(new MakeBuiltinFunctionOperation(parentScope, program)
            {
                Name = definition.Name,
                Definition = definition,
                Func = func,
                PoiLocation = SourceLocation.Identity,
                SourceRange = SourceRange.Identity
            });
        }
    }

    public static IEnumerable<(Definition, BuiltinFunction)> MakeDefinition(System.Type type, Scope parentScope,
        PseudoProgram program)
    {
        var methods = type.GetMethods();
        foreach (var methodInfo in methods)
        {
            if (MakeDefinitionOfMethod(parentScope, program, methodInfo, out var definition))
                yield return (definition, (BuiltinFunction)methodInfo.CreateDelegate(typeof(BuiltinFunction)));
        }
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

        var paramList = GetMethodParamList(parentScope, program, methodInfo);
        var returnDef = GetMethodReturnDefinition(parentScope, program, methodInfo);

        definition = new Definition(parentScope, program)
        {
            Name = functionName,
            References = new List<SourceRange>(),
            SourceRange = SourceRange.Identity,
            Type = new BuiltinFunctionType(parentScope, program)
            {
                ParameterInfos = paramList.ToArray(),
                ReturnType = returnDef
            }
        };
        return true;
    }

    private static string GetFunctionName(MethodInfo methodInfo)
    {
        if (methodInfo.GetCustomAttributes(typeof(BuiltinFunctionAttribute)) is BuiltinFunctionAttribute[] nameAttrs &&
            nameAttrs.Length != 0)
        {
            return nameAttrs[0].Name;
        }

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
}
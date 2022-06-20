using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.Runtime.Reflection;

[AttributeUsage(AttributeTargets.Method)]
public class BuiltinFunctionAttribute : Attribute
{
    public FunctionType.ParameterInfo[] ParameterInfos;
    public Definition ReturnType;

    public BuiltinFunctionAttribute(FunctionType.ParameterInfo[] parameterInfos, Definition returnType)
    {
        ParameterInfos = parameterInfos;
    }
}
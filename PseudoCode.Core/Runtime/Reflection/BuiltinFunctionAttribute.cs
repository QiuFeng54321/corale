using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.Runtime.Reflection;

[AttributeUsage(AttributeTargets.Method)]
public class BuiltinFunctionAttribute : Attribute
{
    public Definition[] ParameterInfos;
    public Definition ReturnType;

    public BuiltinFunctionAttribute(Definition[] parameterInfos, Definition returnType)
    {
        ParameterInfos = parameterInfos;
    }
}
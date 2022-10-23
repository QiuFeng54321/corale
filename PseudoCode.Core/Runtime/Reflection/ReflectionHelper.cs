using System.Reflection;

namespace PseudoCode.Core.Runtime.Reflection;

public static class ReflectionHelper
{
    public static T GetAttribute<T>(MemberInfo memberInfo)
    {
        if (memberInfo.GetCustomAttributes(typeof(T)) is T[]
                nativeNameAttrs &&
            nativeNameAttrs.Length != 0)
            return nativeNameAttrs[0];

        return default;
    }

    public static T GetAttribute<T>(ParameterInfo parameterInfo)
    {
        if (parameterInfo.GetCustomAttributes(typeof(T)) is T[]
                nativeNameAttrs &&
            nativeNameAttrs.Length != 0)
            return nativeNameAttrs[0];

        return default;
    }

    public static bool TryGetAttribute<T>(MemberInfo memberInfo, out T res)
    {
        res = GetAttribute<T>(memberInfo);
        return res != null;
    }

    public static bool TryGetAttribute<T>(ParameterInfo parameterInfo, out T res)
    {
        res = GetAttribute<T>(parameterInfo);
        return res != null;
    }
}
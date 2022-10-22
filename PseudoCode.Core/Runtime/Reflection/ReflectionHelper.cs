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
}
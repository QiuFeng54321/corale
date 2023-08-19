namespace PseudoCode.Core.Runtime.Reflection;

[AttributeUsage(AttributeTargets.Method)]
public class BuiltinNativeFunctionAttribute : Attribute
{
    public readonly string Name;

    public BuiltinNativeFunctionAttribute(string name)
    {
        Name = name;
    }
}
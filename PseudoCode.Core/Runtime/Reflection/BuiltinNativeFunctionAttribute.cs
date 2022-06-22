namespace PseudoCode.Core.Runtime.Reflection;

[AttributeUsage(AttributeTargets.Method)]
public class BuiltinNativeFunctionAttribute : Attribute
{
    public string Name;

    public BuiltinNativeFunctionAttribute(string name)
    {
        Name = name;
    }
}
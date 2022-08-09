namespace PseudoCode.Core.Runtime.Reflection;

[AttributeUsage(AttributeTargets.Method)]
public class BuiltinNativeFunctionAttribute : System.Attribute
{
    public string Name;

    public BuiltinNativeFunctionAttribute(string name)
    {
        Name = name;
    }
}
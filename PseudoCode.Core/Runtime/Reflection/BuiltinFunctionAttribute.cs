namespace PseudoCode.Core.Runtime.Reflection;

[AttributeUsage(AttributeTargets.Method)]
public class BuiltinFunctionAttribute : System.Attribute
{
    public string Name;

    public BuiltinFunctionAttribute(string name)
    {
        Name = name;
    }
}
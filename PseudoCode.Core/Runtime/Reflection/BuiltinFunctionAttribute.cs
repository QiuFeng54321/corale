namespace PseudoCode.Core.Runtime.Reflection;

[AttributeUsage(AttributeTargets.Method)]
public class BuiltinFunctionAttribute : Attribute
{
    public readonly string Name;

    public BuiltinFunctionAttribute(string name)
    {
        Name = name;
    }
}
namespace PseudoCode.Core.Runtime.Reflection;

[AttributeUsage(AttributeTargets.Struct)]
public class BuiltinTypeAttribute : Attribute
{
    public string Name;

    public BuiltinTypeAttribute(string name)
    {
        Name = name;
    }
}
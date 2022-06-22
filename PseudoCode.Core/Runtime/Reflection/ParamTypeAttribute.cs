namespace PseudoCode.Core.Runtime.Reflection;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class ParamTypeAttribute : TypeRepresentation
{
    public string Name;

    public ParamTypeAttribute(string name, string elementTypeName, int dimensionCount = 0, bool isReference = false) :
        base(elementTypeName, dimensionCount, isReference)
    {
        Name = name;
    }
}
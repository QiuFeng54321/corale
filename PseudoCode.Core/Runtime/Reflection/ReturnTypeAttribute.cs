namespace PseudoCode.Core.Runtime.Reflection;

[AttributeUsage(AttributeTargets.Method)]
public class ReturnTypeAttribute : TypeRepresentation
{
    public ReturnTypeAttribute(string elementTypeName, int dimensionCount = 0, bool isReference = false) : base(
        elementTypeName, dimensionCount, isReference)
    {
    }
}
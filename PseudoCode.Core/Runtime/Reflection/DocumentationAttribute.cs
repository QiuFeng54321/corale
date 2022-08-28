namespace PseudoCode.Core.Runtime.Reflection;

[AttributeUsage(AttributeTargets.Method)]
public class DocumentationAttribute : Attribute
{
    public string Documentation;

    public DocumentationAttribute(string documentation)
    {
        Documentation = documentation;
    }
}
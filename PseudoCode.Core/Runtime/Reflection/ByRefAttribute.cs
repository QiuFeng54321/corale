namespace PseudoCode.Core.Runtime.Reflection;

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
public class ByRefAttribute : Attribute
{
}
using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime.Types.Descriptor;

public record PointerDescriptor(ITypeDescriptor TypeDescriptor) : ITypeDescriptor
{
    public Definition GetDefinition(Scope scope, PseudoProgram program)
    {
        return new Definition(scope, program)
        {
            Type = new PointerType(scope, program)
            {
                PointedType = TypeDescriptor.GetType(scope, program)
            },
            Attributes = Definition.Attribute.Type
        };
    }

    public string SelfName => "POINTER";
    public string ToMarkupString() => ToString();

    public override string ToString() => $"^{TypeDescriptor.ToMarkupString()}";
}
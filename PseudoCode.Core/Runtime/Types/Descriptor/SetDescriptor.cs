using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime.Types.Descriptor;

public record SetDescriptor(ITypeDescriptor ElementDescriptor) : ITypeDescriptor
{
    public Definition GetDefinition(Scope scope, PseudoProgram program)
    {
        return new Definition(scope, program)
        {
            Type = new SetType(scope, program)
            {
                ElementType = ElementDescriptor.GetType(scope, program)
            },
            Attributes = Definition.Attribute.Type
        };
    }

    public string SelfName => "SET";

    public string ToMarkupString()
    {
        return $"{{{ElementDescriptor.ToMarkupString()}}}";
    }

    public override string ToString()
    {
        return $"SET OF {ElementDescriptor}";
    }
}
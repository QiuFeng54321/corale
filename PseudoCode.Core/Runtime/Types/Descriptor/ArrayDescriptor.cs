using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime.Types.Descriptor;

public record ArrayDescriptor(ITypeDescriptor ElementDescriptor, int Dimensions) : ITypeDescriptor
{
    public Definition GetDefinition(Scope scope, PseudoProgram program)
    {
        return new Definition(scope, program)
        {
            Type = new ArrayType(scope, program)
            {
                ElementType = ElementDescriptor.GetType(scope, program),
                DimensionCount = Dimensions
            },
            Attributes = Definition.Attribute.Type
        };
    }

    public string SelfName => "ARRAY";
    public string ToMarkupString() => $"{ElementDescriptor.ToMarkupString()}[{Dimensions}]";
    public override string ToString() => string.Format(strings.ArrayType_ToString, Dimensions, ElementDescriptor);
}
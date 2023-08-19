using PseudoCode.Core.Runtime.Types.Descriptor;

namespace PseudoCode.Core.Runtime.Reflection;

public class TypeRepresentation : Attribute
{
    public readonly int DimensionCount;
    public readonly string ElementTypeName;
    public readonly bool IsReference;
    public readonly bool IsSet;

    public TypeRepresentation(string elementTypeName, int dimensionCount = 0, bool isReference = false,
        bool isSet = false)
    {
        ElementTypeName = elementTypeName;
        DimensionCount = dimensionCount;
        IsReference = isReference;
        IsSet = isSet;
    }

    public ITypeDescriptor MakeTypeDescriptor()
    {
        if (DimensionCount > 0)
            return new ArrayDescriptor(new PlainTypeDescriptor(ElementTypeName), DimensionCount);
        if (IsSet)
            return new SetDescriptor(new PlainTypeDescriptor(ElementTypeName));
        return new PlainTypeDescriptor(ElementTypeName);
    }
}
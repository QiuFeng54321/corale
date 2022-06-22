using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.Runtime.Reflection;

public class TypeRepresentation : Attribute
{
    public string ElementTypeName;
    public int DimensionCount;
    public bool IsReference;

    public TypeRepresentation(string elementTypeName, int dimensionCount = 0, bool isReference = false)
    {
        ElementTypeName = elementTypeName;
        DimensionCount = dimensionCount;
        IsReference = isReference;
    }

    public TypeDescriptor MakeTypeDescriptor()
    {
        if (DimensionCount > 0)
            return new TypeDescriptor("ARRAY", Dimensions: DimensionCount,
                ElementType: new TypeDescriptor(ElementTypeName));
        return new TypeDescriptor(ElementTypeName);
    }
}
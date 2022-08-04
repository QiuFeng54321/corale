using PseudoCode.Core.Runtime.Types;
using PseudoCode.Core.Runtime.Types.Descriptor;

namespace PseudoCode.Core.Runtime.Reflection;

public class TypeRepresentation : Attribute
{
    public int DimensionCount;
    public string ElementTypeName;
    public bool IsReference;

    public TypeRepresentation(string elementTypeName, int dimensionCount = 0, bool isReference = false)
    {
        ElementTypeName = elementTypeName;
        DimensionCount = dimensionCount;
        IsReference = isReference;
    }

    public ITypeDescriptor MakeTypeDescriptor()
    {
        if (DimensionCount > 0)
            return new ArrayDescriptor(new PlainTypeDescriptor(ElementTypeName), DimensionCount);
        return new PlainTypeDescriptor(ElementTypeName);
    }
}
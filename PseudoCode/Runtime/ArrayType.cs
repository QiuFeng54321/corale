namespace PseudoCode.Runtime;

public class ArrayType : Type
{
    public override string Name => "ARRAY";
    public override uint Id => ArrayId;

    public override Instance Instance(object value = null)
    {
        return new ArrayInstance();
    }

    public Instance Instance(List<Range> dimensions, Type elementType)
    {
        var i = (ArrayInstance)Instance();
        i.Dimensions = dimensions;
        i.ElementType = elementType;
        i.Initialise();
        return i;
    }
}
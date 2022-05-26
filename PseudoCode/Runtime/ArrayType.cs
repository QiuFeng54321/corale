namespace PseudoCode.Runtime;

public class ArrayType : Type
{
    public override string Name => "ARRAY";
    public override uint Id => ArrayId;

    public override Instance Instance(object value = null)
    {
        return new ArrayInstance(Scope, Program)
        {
            Type = this,
            Members = new Dictionary<string, Instance>(),
            Value = value
        };
    }

    public ArrayInstance Instance(List<Range> dimensions, Type elementType)
    {
        var i = (ArrayInstance)Instance();
        i.Dimensions = dimensions;
        i.ElementType = elementType;
        i.InitialiseArray();
        return i;
    }

    public override void Assign(Instance to, Instance value)
    {
        for (var i = 0; i < ((Instance[])value.Value).Length; i++)
        {
            ((Instance[])to.Value)[i].Type.Assign(((Instance[])to.Value)[i], ((Instance[])value.Value)[i]);
        }
    }

    public override Instance Index(Instance i1, Instance i2)
    {
        return base.Index(i1, i2);
    }
}
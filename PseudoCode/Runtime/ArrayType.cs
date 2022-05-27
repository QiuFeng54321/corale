using PseudoCode.Runtime.Errors;

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
        if (value.Type.Id != ArrayId)
            throw new InvalidTypeError(
                $"Assigned value should be {Name} OF {((ArrayInstance)to).ElementType}, not {value.Type}", null);
        var valueLength = ((Instance[])value.Value).Length;
        var toLength = ((Instance[])to.Value).Length;
        if (valueLength != toLength)
            throw new InvalidAccessError($"Assigning array of length {valueLength} to array of length {toLength}",
                null);
        for (var i = 0; i < valueLength; i++)
            ((Instance[])to.Value)[i].Type.Assign(((Instance[])to.Value)[i], ((Instance[])value.Value)[i]);
    }

    public override Instance Index(Instance i1, Instance i2)
    {
        if (i2.RealInstance is not ArrayInstance indexInstance)
            throw new InvalidAccessError("Index access is not an array!", null);
        var arrayInstance = (ArrayInstance)i1.RealInstance;
        if (indexInstance.TotalElements > arrayInstance.Dimensions.Count)
            throw new InvalidAccessError(
                $"Array access indices more than dimensions of array: {indexInstance.TotalElements} > {arrayInstance.Dimensions.Count}",
                null);
        var indexList = indexInstance.Array.Select((index, i) =>
            arrayInstance.Dimensions[i].ToRealIndex(Scope.FindType("INTEGER").HandledCastFrom(index).Get<int>())).ToList();
        return arrayInstance.ElementAt(indexList);
    }
}
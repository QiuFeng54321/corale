using PseudoCode.Core.Runtime.Errors;
using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime;

public class ArrayType : Type
{
    public override string Name => "ARRAY";
    public override uint Id => ArrayId;

    public override Instance Instance(object value = null, Scope scope = null)
    {
        return new ArrayInstance(scope ?? ParentScope, Program)
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

    public override Instance Clone(Instance instance)
    {
        var arrayInstance = (ArrayInstance)instance;
        var cloned = Instance(arrayInstance.Dimensions, arrayInstance.ElementType);
        cloned.Array = arrayInstance.Array;
        return cloned;
    }

    public override Instance Assign(Instance to, Instance value)
    {
        if (value.Type.Id != ArrayId)
            throw new InvalidTypeError(
                string.Format(strings.ArrayType_Assign_InvalidValueType, Name, ((ArrayInstance)to.RealInstance).ElementType, value.Type), null);
        var valueLength = value.Get<Instance[]>().Length;
        var toLength = to.Get<Instance[]>().Length;
        if (valueLength != toLength)
            throw new InvalidAccessError(string.Format(strings.ArrayType_Assign_InvalidArrayLength, valueLength, toLength),
                null);
        for (var i = 0; i < valueLength; i++)
            to.Get<Instance[]>()[i].Type.Assign(to.Get<Instance[]>()[i], value.Get<Instance[]>()[i]);
        return to;
    }

    public override Instance Index(Instance i1, Instance i2)
    {
        if (i2.RealInstance is not ArrayInstance indexInstance)
            throw new InvalidAccessError(strings.ArrayType_Index_IndexNotArray, null);
        var arrayInstance = (ArrayInstance)i1.RealInstance;
        if (indexInstance.TotalElements > arrayInstance.Dimensions.Count)
            throw new InvalidAccessError(
                string.Format(strings.ArrayType_Index_InvalidArrayAccessDimension, indexInstance.TotalElements, arrayInstance.Dimensions.Count),
                null);
        var indexList = indexInstance.Array.Select((index, i) =>
            arrayInstance.Dimensions[i].ToRealIndex(ParentScope.FindType(IntegerId).HandledCastFrom(index).Get<int>())).ToList();
        return arrayInstance.ElementAt(indexList);
    }
}
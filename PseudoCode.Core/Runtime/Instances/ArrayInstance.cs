using Newtonsoft.Json;
using PseudoCode.Core.Runtime.Errors;
using PseudoCode.Core.Runtime.Operations;
using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.Runtime.Instances;

public class ArrayInstance : Instance
{
    public Instance[] Array;
    public List<Range> Dimensions = new();
    public uint StartAddress;

    public ArrayInstance(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public ArrayInstance()
    {
    }

    public ArrayType ArrayType => (ArrayType)Type;
    public int TotalElements => Dimensions.Select(d => d.Length).Aggregate((prev, next) => prev * next);

    [JsonIgnore]
    public override object Value
    {
        get => Array;
        set => Array = (Instance[])value;
    }

    public void InitialiseArray()
    {
        Array = new Instance[TotalElements];
    }

    public void InitialiseInMemory()
    {
        InitialiseArray();
        StartAddress = Program.Allocate(TotalElements, () => ArrayType.ElementType.Instance(scope: ParentScope));
        InitialiseAsReferenceElements();
    }

    public void InitialiseAsReferenceElements()
    {
        for (var i = 0u; i < Array.Length; i++)
            Array[i] = new ReferenceInstance(ParentScope, Program) { ReferenceAddress = StartAddress + i };
    }

    public void InitialiseNonReference()
    {
        for (var i = 0u; i < Array.Length; i++)
            Array[i] = ((ArrayType)Type).ElementType.Instance(scope: ParentScope);
    }

    public void InitialiseFromList(IEnumerable<Instance> instances)
    {
        Array = instances.ToArray();
    }


    public Instance ElementAt(int index)
    {
        if (Array.Length <= index || index < 0)
            throw new OutOfBoundsError(string.Format(strings.ArrayInstance_ElementAt_OutOfBounds, index, TotalElements),
                null);
        return Array[index];
    }

    public Instance ElementAt(IEnumerable<int> indices)
    {
        var enumerable = indices as int[] ?? indices.ToArray();
        if (enumerable.Length > ArrayType.DimensionCount)
            throw new InvalidAccessError(
                string.Format(strings.ArrayType_Index_InvalidArrayAccessDimension, TotalElements, enumerable.Length),
                null);
        var index = 0;
        var factor = TotalElements;
        for (var i = 0; i < enumerable.Length; i++)
        {
            factor /= Dimensions[i].Length;
            index += enumerable[i] * factor;
        }

        if (enumerable.Length == ArrayType.DimensionCount)
            return ElementAt(index);

        var newArrayType = new ArrayType(ParentScope, Program)
        {
            DimensionCount = ArrayType.DimensionCount - enumerable.Length,
            ElementType = ArrayType.ElementType
        };
        var arrayInstance = (ArrayInstance)newArrayType.Instance();
        arrayInstance.Dimensions = Dimensions.Skip(enumerable.Length).ToList();
        arrayInstance.StartAddress = (uint)index + StartAddress;
        arrayInstance.InitialiseArray();
        arrayInstance.InitialiseAsReferenceElements();
        return arrayInstance;
    }

    public override string Represent()
    {
        return $"[{string.Join<Instance>(", ", Array)}]";
    }

    public override string DebugRepresent()
    {
        return $"{{{((ArrayType)Type).ElementType}[{string.Join(", ", Dimensions)}]: {Represent()}}}";
    }
}
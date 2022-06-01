using PseudoCode.Core.Runtime.Errors;
using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime;

public class ArrayInstance : Instance
{
    public Instance[] Array;
    public uint StartAddress;
    public ArrayType ArrayType => (ArrayType)Type;

    public ArrayInstance(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override object Value
    {
        get => Array;
        set => Array = (Instance[])value;
    }
    public void InitialiseArray()
    {
        Array = new Instance[ArrayType.TotalElements];
    }

    public void InitialiseInMemory()
    {
        InitialiseArray();
        StartAddress = Program.Allocate(ArrayType.TotalElements, () => ((ArrayType)Type).ElementType.Instance(scope: ParentScope));
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
            throw new OutOfBoundsError(string.Format(strings.ArrayInstance_ElementAt_OutOfBounds, index, ArrayType.TotalElements), null);
        return Array[index];
    }

    public Instance ElementAt(IEnumerable<int> indices)
    {
        var enumerable = indices as int[] ?? indices.ToArray();
        if (enumerable.Length > ((ArrayType)Type).Dimensions.Count)
            throw new InvalidAccessError(
                string.Format(strings.ArrayType_Index_InvalidArrayAccessDimension, ArrayType.TotalElements, enumerable.Length),
                null);
        var index = 0;
        var factor = ArrayType.TotalElements;
        for (var i = 0; i < enumerable.Length; i++)
        {
            factor /= ((ArrayType)Type).Dimensions[i].Length;
            index += enumerable[i] * factor;
        }

        if (enumerable.Length == ((ArrayType)Type).Dimensions.Count)
            return ElementAt(index);

        var newArrayType = new ArrayType (ParentScope, Program)
        {
            Dimensions = ArrayType.Dimensions.Skip(enumerable.Length).ToList(),
            ElementType = ArrayType.ElementType
        };
        var arrayInstance = (ArrayInstance)newArrayType.Instance();
        arrayInstance.StartAddress = (uint)index + StartAddress;
        arrayInstance.InitialiseAsReferenceElements();
        return arrayInstance;
    }

    public override string Represent()
    {
        return $"[{string.Join<Instance>(", ", Array)}]";
    }

    public override string DebugRepresent()
    {
        return $"{{{((ArrayType)Type).ElementType}[{string.Join(", ", ((ArrayType)Type).Dimensions)}]: {Represent()}}}";
    }
}
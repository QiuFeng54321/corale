using PseudoCode.Runtime.Errors;
using PseudoCode.Runtime.Operations;

namespace PseudoCode.Runtime;

public class ArrayInstance : Instance
{
    public Instance[] Array;
    public List<Range> Dimensions = new();
    public Type ElementType;
    public uint StartAddress;

    public ArrayInstance(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override object Value
    {
        get => Array;
        set => Array = (Instance[])value;
    }

    public int TotalElements => Dimensions.Select(d => d.Length).Aggregate((prev, next) => prev * next);

    public void InitialiseArray()
    {
        Array = new Instance[TotalElements];
    }

    public void InitialiseInMemory()
    {
        StartAddress = Program.Allocate(TotalElements, () => ElementType.Instance(scope: ParentScope));
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
            Array[i] = ElementType.Instance(scope: ParentScope);
    }

    public void InitialiseFromList(IEnumerable<Instance> instances)
    {
        Array = instances.ToArray();
    }


    public Instance ElementAt(int index)
    {
        return Array[index];
    }

    public Instance ElementAt(IEnumerable<int> indices)
    {
        var enumerable = indices as int[] ?? indices.ToArray();
        if (enumerable.Length > Dimensions.Count)
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

        if (enumerable.Length == Dimensions.Count)
            return ElementAt(index);


        var arrayInstance = ((ArrayType)Type).Instance(Dimensions.Skip(enumerable.Length).ToList(), ElementType);
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
        return $"{{{ElementType}[{string.Join(", ", Dimensions)}]: {Represent()}}}";
    }
}
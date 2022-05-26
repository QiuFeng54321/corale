using PseudoCode.Runtime.Operations;

namespace PseudoCode.Runtime;

public class ArrayInstance : Instance
{
    public Instance[] Array;
    public List<Range> Dimensions = new();
    public Type ElementType;
    public uint StartAddress;

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
        StartAddress = Program.Allocate(TotalElements, () => ElementType.Instance());
        for (var i = 0u; i < Array.Length; i++)
            Array[i] = new ReferenceInstance(ParentScope, Program) { ReferenceAddress = StartAddress + i };
    }

    public void InitialiseNonReference()
    {
        for (var i = 0u; i < Array.Length; i++)
            Array[i] = ElementType.Instance();
    }

    public void InitialiseFromList(IEnumerable<Instance> instances)
    {
        Array = instances.ToArray();
    }


    public Instance ElementAt(int index)
    {
        return Array[Dimensions[0].ToRealIndex(index)];
    }

    public override string Represent() => $"[{string.Join<Instance>(',', Array)}]";

    public override string DebugRepresent()
    {
        return $"{{{ElementType}[{string.Join(',', Dimensions)}]: {Represent()}}}";
    }

    public ArrayInstance(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }
}
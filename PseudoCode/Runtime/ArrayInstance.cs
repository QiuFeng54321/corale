namespace PseudoCode.Runtime;

public class ArrayInstance : Instance
{
    public Instance[] Array;
    public List<Range> Dimensions = new();
    public Type ElementType;

    public override object Value
    {
        get => Array;
        set => Array = (Instance[])value;
    }

    public void Initialise()
    {
        Array = new Instance[Dimensions[0].Length];
        for (var i = 0; i < Array.Length; i++)
            if (Dimensions.Count > 1)
                Array[i] = ((ArrayType)Type).Instance(Dimensions.TakeLast(Dimensions.Count - 1).ToList(), ElementType);
            else
                Array[i] = ElementType.Instance();
    }

    public Instance ElementAt(int index)
    {
        return Array[Dimensions[0].ToRealIndex(index)];
    }

    public override string ToString()
    {
        return $"{{{ElementType}[{string.Join(',',Dimensions)}]: [{string.Join<Instance>(',', Array)}]}}";
    }
}
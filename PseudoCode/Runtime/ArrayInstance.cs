namespace PseudoCode.Runtime;

public class ArrayInstance : Instance
{
    public List<Range> Dimensions = new();
    public Instance[] Array;
    public Type ElementType;

    public void Initialise()
    {
        Array = new Instance[Dimensions[0].Length];
        for (var i = 0; i < Array.Length; i++)
        {
            if (Dimensions.Count > 1)
            {
                Array[i] = ((ArrayType)Type).Instance(Dimensions.TakeLast(Dimensions.Count - 1).ToList(), ElementType);
            }
            else
            {
                Array[i] = ElementType.Instance();
            }
        }
    }

    public override object Value { get => Array; set => Array = (Instance[])value; }
    public Instance ElementAt(int index) => Array[Dimensions[0].ToRealIndex(index)];
}
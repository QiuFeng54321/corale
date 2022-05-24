namespace PseudoCode.Runtime.Operations;

public class DeclareOperation : Operation
{
    public List<Range> Dimensions = new();
    public string Name;
    public Type Type;

    public override void Operate()
    {
        base.Operate();
        if (Scope.Instances.ContainsKey(Name))
            throw new InvalidOperationException($"{Name} is already declared under this scope!");
        if (Dimensions.Count == 0)
        {
            Scope.Instances.Add(Name, Type.Instance());
        }
        else
        {
            var arrayInstance = ((ArrayType)Scope.FindType("ARRAY")).Instance(Dimensions, Type);
            Scope.Instances.Add(Name, arrayInstance);
        }
    }

    public override string ToString()
    {
        var typeStr = Dimensions.Count == 0 ? Type.ToString() : $"ARRAY[{string.Join(',', Dimensions)}] OF {Type}";
        return $"DECLARE {Name} : {typeStr}";
    }
}
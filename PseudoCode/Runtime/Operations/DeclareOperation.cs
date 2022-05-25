namespace PseudoCode.Runtime.Operations;

public class DeclareOperation : Operation
{
    public List<Range> Dimensions = new();
    public string Name;
    public Type Type;

    public override void Operate()
    {
        base.Operate();
        if (ParentScope.InstanceAddresses.ContainsKey(Name))
            throw new InvalidOperationException($"{Name} is already declared under this scope!");
        if (Dimensions.Count == 0)
        {
            ParentScope.InstanceAddresses.Add(Name, Program.AllocateId(Type.Instance()));
        }
        else
        {
            var arrayInstance = ((ArrayType)ParentScope.FindType("ARRAY")).Instance(Dimensions, Type);
            ParentScope.InstanceAddresses.Add(Name, Program.AllocateId(arrayInstance));
        }
    }

    public override string ToString()
    {
        var typeStr = Dimensions.Count == 0 ? Type.ToString() : $"ARRAY[{string.Join(',', Dimensions)}] OF {Type}";
        return $"DECLARE {Name} : {typeStr}";
    }

    public DeclareOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }
}
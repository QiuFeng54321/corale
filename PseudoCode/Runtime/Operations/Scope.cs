namespace PseudoCode.Runtime.Operations;

public class Scope : Operation
{
    public Dictionary<string, Instance> Instances = new();
    public List<Operation> Operations = new();
    public Scope Parent;
    public Stack<Instance> RuntimeStack = new();
    public Dictionary<string, Type> Types = new();

    public Instance FindInstance(string name)
    {
        return Instances.ContainsKey(name) ? Instances[name] : Parent?.FindInstance(name);
    }

    public Type FindType(string typeName)
    {
        return Types.ContainsKey(typeName) ? Types[typeName] : Parent?.FindType(typeName);
    }
    public Type FindType(uint id)
    {
        var t = Types.FirstOrDefault(t => t.Value.Id == id, new KeyValuePair<string, Type>());
        return t.Value ?? Parent?.FindType(id);
    }

    public Scope AddScope()
    {
        return new() { Parent = this };
    }

    public void AddType(string name, Type type)
    {
        type.ParentScope = this;
        Types.Add(name, type);
    }

    public void AddOperation(Operation operation)
    {
        Operations.Add(operation);
    }

    public Operation Take(int i)
    {
        var res = Operations[i];
        Operations.RemoveAt(i);
        return res;
    }

    public Operation TakeFirst() => Take(0);
    public Operation TakeLast() => Take(Operations.Count - 1);

    public override void Operate()
    {
        base.Operate();
        foreach (var operation in Operations) operation.Operate();
    }

    public override string ToString()
    {
        return $"Scope\n\t{string.Join("\n\t", Operations)}\nUnscope";
    }
}
namespace PseudoCode.Runtime.Operations;

public class Scope : Operation
{
    public Scope Parent;
    public Dictionary<string, Instance> Instances = new();
    public Stack<Instance> RuntimeStack = new();
    public Queue<Operation> Operations = new();
    public Dictionary<string, Type> Types = new();

    public Instance FindInstance(string name) =>
        Instances.ContainsKey(name) ? Instances[name] : Parent?.FindInstance(name);

    public Type FindType(string typeName) => Types.ContainsKey(typeName) ? Types[typeName] : Parent?.FindType(typeName);

    public Scope AddScope() => new Scope { Parent = this };

    public void AddType(string name, Type type)
    {
        type.ParentScope = this;
        Types.Add(name, type);
    }
    public override void Operate()
    {
        base.Operate();
        foreach (var operation in Operations)
        {
            operation.Operate();
        }
    }

    public override string ToString()
    {
        return $"Scope\n{string.Join('\n', Operations)}\nUnscope";
    }
}
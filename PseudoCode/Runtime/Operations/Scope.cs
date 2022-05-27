using PseudoCode.Runtime.Errors;

namespace PseudoCode.Runtime.Operations;

public class Scope : Operation
{
    public Dictionary<string, uint> InstanceAddresses = new();
    public List<Operation> Operations = new();
    public Stack<Instance> RuntimeStack = new();
    public Dictionary<string, Type> Types = new();

    public Instance FindInstance(string name)
    {
        return ParentScope.Program.Memory[FindInstanceAddress(name)];
    }

    public uint FindInstanceAddress(string name)
    {
        return InstanceAddresses.ContainsKey(name)
            ? InstanceAddresses[name]
            : ParentScope?.FindInstanceAddress(name) ??
              throw new InvalidAccessError($"Instance {name} cannot be found.", null);
    }

    public Type FindType(string typeName)
    {
        return Types.ContainsKey(typeName) ? Types[typeName] : ParentScope?.FindType(typeName);
    }

    public Type FindType(uint id)
    {
        var t = Types.FirstOrDefault(t => t.Value.Id == id, new KeyValuePair<string, Type>());
        return t.Value ?? ParentScope?.FindType(id);
    }

    public Scope AddScope()
    {
        return new Scope(this, Program);
    }

    public void AddType(string name, Type type)
    {
        type.Scope = this;
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
        foreach (var operation in Operations)
        {
            try
            {
                operation.Operate();
            }
            catch (Error e)
            {
                e.Operation = operation;
                e.OperationStackTrace.Add(this);
                throw;
            }
        }
    }

    public override string ToPlainString() => ParentScope == null ? "Global scope" : "Anonymous scope";

    public override string ToString(int depth)
    {
        return $"{Indent(depth)}{{\n{string.Join("\n", Operations.Select(o => o.ToString(depth + 1)))}\n{Indent(depth)}}}";
    }

    public Scope(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }
}
using PseudoCode.Core.Runtime.Errors;

namespace PseudoCode.Core.Runtime.Operations;

public class Scope : Operation
{
    public bool IsRoot => ParentScope == null;

    public Scope(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
        ScopeStates = new ScopeStates();
    }

    public ScopeStates ScopeStates { get; set; }
    
    /// <summary>
    /// Instances are created from the type
    /// </summary>
    public Dictionary<string, Type> InstanceTypes = new();
    public Dictionary<string, Type> Types = new();
    public Type FindInstanceType(string name)
    {
        return InstanceTypes.ContainsKey(name) ? InstanceTypes[name] : ParentScope?.FindInstanceType(name);
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

    public void RegisterInstanceType(string name, Type type)
    {
        InstanceTypes.Add(name, type);
    }
    public Instance FindInstance(string name)
    {
        return ParentScope.Program.Memory[FindInstanceAddress(name)];
    }

    public uint FindInstanceAddress(string name)
    {
        return ScopeStates.InstanceAddresses.ContainsKey(name)
            ? ScopeStates.InstanceAddresses[name]
            : ParentScope?.FindInstanceAddress(name) ??
              throw new InvalidAccessError(string.Format(strings.Scope_FindInstanceAddress_NotFound, name), null);
    }

    public Scope FindScope(SourceLocation location)
    {
        if (!SourceRange.Contains(location)) return null;

        return ScopeStates.Operations
            .Where(o => o is Scope)
            .Select(o => ((Scope)o).FindScope(location))
            .FirstOrDefault(s => s != null, this);
    }

    public Scope AddScope(SourceLocation sourceLocation = default)
    {
        return new Scope(this, Program)
        {
            PoiLocation = sourceLocation,
            SourceRange = new SourceRange(PoiLocation, null)
        };
    }

    public void AddType(Type type)
    {
        type.ParentScope = this;
        Types.Add(type.Name, type);
    }

    public void AddOperation(Operation operation)
    {
        ScopeStates.Operations.Add(operation);
    }

    public Operation Take(int i)
    {
        var res = ScopeStates.Operations[i];
        ScopeStates.Operations.RemoveAt(i);
        return res;
    }

    public Operation TakeFirst()
    {
        return Take(0);
    }

    public Operation TakeLast()
    {
        return Take(ScopeStates.Operations.Count - 1);
    }

    public override void Operate()
    {
        base.Operate();
        if (IsRoot) // Do not remove temp contents. There are predefined types
        {
            RunOperations();
            return;
        }

        var copy = (ScopeStates)ScopeStates.Clone();
        ScopeStates.ResetTemporaryContent();
        RunOperations();
        ScopeStates = copy; // Reset
    }

    public override void MetaOperate()
    {
        base.MetaOperate();
        foreach (var operation in ScopeStates.Operations)
            operation.MetaOperate();
    }

    private void RunOperations()
    {
        foreach (var operation in ScopeStates.Operations)
            try
            {
                operation.HandledOperate();
            }
            catch (Error e)
            {
                // e.Operation ??= operation;
                e.OperationStackTrace.Add(this);
                if (!IsRoot) throw;
                Console.Error.WriteLine(e);
                return;
            }
    }

    public override string ToPlainString()
    {
        return ParentScope == null
            ? strings.Scope_ToPlainString_GlobalScope
            : strings.Scope_ToPlainString_AnonymousScope;
    }

    public override string ToString(int depth)
    {
        return
            $"{Indent(depth)}{{\n{string.Join("\n", ScopeStates.Operations.Select(o => o.ToString(depth + 1)))}\n{Indent(depth)}}}";
    }
}
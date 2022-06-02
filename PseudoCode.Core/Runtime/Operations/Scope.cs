using System.Collections;
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
    public List<Scope> ChildScopes { get; set; } = new();

    /// <summary>
    /// Instances are created from the type
    /// </summary>
    public Dictionary<string, Definition> InstanceDefinitions = new();

    public Dictionary<string, Definition> TypeDefinitions = new();

    public Definition FindInstanceDefinition(string name)
    {
        return InstanceDefinitions.ContainsKey(name)
            ? InstanceDefinitions[name]
            : ParentScope?.FindInstanceDefinition(name);
    }

    public Definition FindTypeDefinition(string typeName)
    {
        return TypeDefinitions.ContainsKey(typeName)
            ? TypeDefinitions[typeName]
            : ParentScope?.FindTypeDefinition(typeName);
    }

    public Definition FindTypeDefinition(uint id)
    {
        var t = TypeDefinitions.FirstOrDefault(t => t.Value.Type.Id == id, new KeyValuePair<string, Definition>());
        return t.Value ?? ParentScope?.FindTypeDefinition(id);
    }

    public void RegisterInstanceType(string name, Type type)
    {
        InstanceDefinitions.Add(name, new Definition
        {
            Name = name, Type = type
        });
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

    public Scope AddScope(SourceLocation sourceLocation = default, SourceRange sourceRange = default)
    {
        var scope = new Scope(this, Program)
        {
            PoiLocation = sourceLocation,
            SourceRange = sourceRange
        };
        ChildScopes.Add(scope);
        return scope;
    }

    public void AddType(Type type)
    {
        type.ParentScope = this;
        TypeDefinitions.Add(type.Name, new Definition
        {
            Name = type.Name,
            Type = type,
            SourceRange = new SourceRange(new SourceLocation(-1, -1), new SourceLocation(-1, -1))
        });
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

    public IEnumerable<Definition> GetAllDefinedVariables()
    {
        var res = InstanceDefinitions
            .Select(x => x.Value);
        res = ChildScopes.Aggregate(res, (current, childScope) => current.Concat(childScope.GetAllDefinedVariables()));
        return res;
    }

    public Definition GetHoveredVariableDefinition(SourceLocation location)
    {
        return GetAllDefinedVariables()
            .FirstOrDefault(d => d.SourceRange.Contains(location) || d.References.Any(r => r.Contains(location)), null);
    }

    public (Definition, SourceRange) GetHoveredVariable(SourceLocation location)
    {
        var hovered = GetHoveredVariableDefinition(location);
        if (hovered == null) return (null, null);
        return hovered.SourceRange.Contains(location)
            ? (hovered, hovered.SourceRange)
            : (hovered, hovered.References.FirstOrDefault(r => r.Contains(location)));
    }

    public IEnumerable<Definition> GetVariableCompletionBefore(SourceLocation location)
    {
        var res = InstanceDefinitions.Where(x => x.Value.SourceRange.End <= location)
            .Select(x => x.Value);

        return ChildScopes.Where(s => s.SourceRange.Contains(location)).Aggregate(res,
            (current, childScope) =>
                current.Concat(childScope.GetVariableCompletionBefore(location) ?? Array.Empty<Definition>()));
    }

    public IEnumerable<Definition> GetTypeCompletionBefore(SourceLocation location)
    {
        var res = TypeDefinitions.Where(x => x.Value.SourceRange.End <= location)
            .Select(x => x.Value);

        return ChildScopes.Where(s => s.SourceRange.Contains(location)).Aggregate(res,
            (current, childScope) =>
                current.Concat(childScope.GetTypeCompletionBefore(location) ?? Array.Empty<Definition>()));
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
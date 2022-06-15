using PseudoCode.Core.Analyzing;
using PseudoCode.Core.Runtime.Errors;
using PseudoCode.Core.Runtime.Instances;
using Type = PseudoCode.Core.Runtime.Types.Type;

namespace PseudoCode.Core.Runtime.Operations;

public class Scope : Operation
{
    /// <summary>
    ///     Instances are created from the type
    /// </summary>
    public Dictionary<string, Definition> InstanceDefinitions = new();

    public Dictionary<string, Definition> TypeDefinitions = new();

    public Scope(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
        ScopeStates = new ScopeStates();
    }

    public bool IsRoot => ParentScope == null;

    public ScopeStates ScopeStates { get; set; }
    public List<Scope> ChildScopes { get; set; } = new();
    public bool AllowStatements;
    public SourceLocation FirstLocation;

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
        TypeDefinitions.Add(type.Name, new Definition (ParentScope, Program)
        {
            Name = type.Name,
            Type = type,
            SourceRange = new SourceRange(new SourceLocation(-1, -1), new SourceLocation(-1, -1))
        });
    }

    public void AddVariableDefinition(string name, Definition definition, SourceRange sourceRange = null)
    {
        if (InstanceDefinitions.ContainsKey(name))
            Program.AnalyserFeedbacks.Add(new Feedback
            {
                Message = $"Variable {name} is already declared",
                Severity = Feedback.SeverityType.Error,
                SourceRange = sourceRange
            });
        else
            InstanceDefinitions.Add(name, definition);
    }
    public void AddTypeDefinition(string name, Definition definition, SourceRange sourceRange = null)
    {
        if (TypeDefinitions.ContainsKey(name))
            Program.AnalyserFeedbacks.Add(new Feedback
            {
                Message = $"Type {name} is already declared",
                Severity = Feedback.SeverityType.Error,
                SourceRange = sourceRange
            });
        else
            TypeDefinitions.Add(name, definition);
    }
    public void AddOperation(Operation operation)
    {
        ScopeStates.Operations.Add(operation);
        if (operation.SourceRange == null) return;
        FirstLocation ??= operation.SourceRange.Start;
        if (FirstLocation > operation.SourceRange.Start) FirstLocation = operation.SourceRange.Start;
    }
    public void InsertOperation(int index, Operation operation)
    {
        ScopeStates.Operations.Insert(index, operation);
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
        Operate(s =>
        {
            s.ResetTemporaryContent();
            return s;
        });
    }

    public void Operate(Func<ScopeStates, ScopeStates> scopeStatesGen)
    {
        if (IsRoot) // Do not remove temp contents. There are predefined types
        {
            RunOperations(ScopeStates);
            return;
        }

        var copy = (ScopeStates)ScopeStates.Clone();
        ScopeStates = scopeStatesGen(ScopeStates);
        RunOperations(copy);
    }

    public void Join(Scope scope)
    {
        scope.ScopeStates.Operations.ForEach(o => o.ParentScope = this);
        ScopeStates.Operations.AddRange(scope.ScopeStates.Operations);
        foreach (var (name, def) in scope.InstanceDefinitions) InstanceDefinitions.Add(name, def);
    }

    public override void MetaOperate()
    {
        base.MetaOperate();
        foreach (var operation in ScopeStates.Operations)
            operation.MetaOperate();
    }

    private void RunOperations(ScopeStates scopeStates)
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
                ScopeStates = scopeStates; // Reset
                if (!IsRoot) throw;
                Console.Error.WriteLine(e);
                return;
            }

        ScopeStates = scopeStates;
    }

    public IEnumerable<Definition> GetAllDefinedVariables()
    {
        var res = InstanceDefinitions
            .Select(x => x.Value);
        res = ChildScopes.Aggregate(res, (current, childScope) => current.Concat(childScope.GetAllDefinedVariables()));
        return res;
    }
    public Scope GetNearestStatementScopeBefore(SourceLocation sourceLocation)
    {
        foreach (var scope in ChildScopes.Where(scope => scope.AllowStatements && scope.SourceRange.Contains(sourceLocation)))
        {
            return scope.GetNearestStatementScopeBefore(sourceLocation);
        }

        return AllowStatements && SourceRange.Contains(sourceLocation) ? this : null;
    }


    public static Definition GetHoveredVariableDefinition(IEnumerable<Definition> definitions, SourceLocation location)
    {
        return definitions
            .FirstOrDefault(d => d.SourceRange.Contains(location) || d.References.Any(r => r.Contains(location)), null);
    }

    public Definition GetHoveredVariableDefinition(SourceLocation location)
    {
        return GetHoveredVariableDefinition(GetAllDefinedVariables(), location);
    }

    public static (Definition, SourceRange) GetHoveredVariable(IEnumerable<Definition> definitions,
        SourceLocation location)
    {
        var hovered = GetHoveredVariableDefinition(definitions, location);
        if (hovered == null) return (null, null);
        return hovered.SourceRange.Contains(location)
            ? (hovered, hovered.SourceRange)
            : (hovered, hovered.References.FirstOrDefault(r => r.Contains(location)));
    }

    public (Definition, SourceRange) GetHoveredVariable(SourceLocation location)
    {
        return GetHoveredVariable(GetAllDefinedVariables(), location);
    }

    public static IEnumerable<Definition> GetDefinitionsBefore(IEnumerable<Definition> definitions,
        SourceLocation location)
    {
        return definitions.Where(x => x.SourceRange.End <= location);
    }

    public IEnumerable<Definition> GetVariableCompletionBefore(SourceLocation location)
    {
        var res = GetDefinitionsBefore(InstanceDefinitions.Values, location);

        return ChildScopes.Where(s => s.SourceRange.Contains(location)).Aggregate(res,
            (current, childScope) =>
                current.Concat(childScope.GetVariableCompletionBefore(location) ?? Array.Empty<Definition>()));
    }

    public IEnumerable<Definition> GetTypeCompletionBefore(SourceLocation location)
    {
        var res = GetDefinitionsBefore(TypeDefinitions.Values, location);

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
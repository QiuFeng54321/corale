namespace PseudoCode.Core.Runtime.Emit;

public class PseudoScope
{
    public ProgramBuilder ProgramBuilder;
    public PseudoScope ParentScope;
    public readonly Dictionary<string, Identifier> Identifiers = new();
    public readonly List<PseudoScope> ChildrenScopes = new();
    public ulong CurrentTempVariableCount = 0;

    public PseudoScope(ProgramBuilder programBuilder, PseudoScope parentScope = default)
    {
        ProgramBuilder = programBuilder;
        ParentScope = parentScope;
    }

    public Identifier GetIdentifier(string name) =>
        Identifiers.GetValueOrDefault(name) ?? ParentScope?.GetIdentifier(name);

    public Identifier GetOrMakeIdentifier(string name)
    {
        var id = GetIdentifier(name);
        if (id != null) return id;
        id = new Identifier(name, this);
        AddIdentifier(name, id);
        return id;
    }

    public void AddIdentifier(string name, Identifier id)
    {
        Identifiers.Add(name, id);
    }

    public Identifier MakeTempIdentifier(TypeDefinition definition)
    {
        var id = ProgramBuilder.MakeIdentifier($"$$TMP{CurrentTempVariableCount++}", definition, this);
        return id;
    }

    public PseudoScope EnterNewScope()
    {
        var scope = new PseudoScope(ProgramBuilder, this);
        ChildrenScopes.Add(scope);
        return scope;
    }

    public IEnumerable<Identifier> GetIdentifiers()
    {
        foreach (var identifier in Identifiers)
        {
            yield return identifier.Value;
        }

        foreach (var childScope in ChildrenScopes)
        {
            foreach (var identifier in childScope.Identifiers)
            {
                yield return identifier.Value;
            }
        }
    }
}
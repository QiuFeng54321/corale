using PseudoCode.Runtime.Operations;

namespace PseudoCode.Runtime;

public class Instance
{
    public virtual Type Type { get; set; }
    public virtual Dictionary<string, Instance> Members { get; init; } = new();
    public virtual object Value { get; set; } = null!;
    public uint InstanceAddress;
    public Scope ParentScope;
    public PseudoProgram Program;

    public Instance(Scope parentScope, PseudoProgram program)
    {
        ParentScope = parentScope;
        Program = program;
    }

    public static Instance Null;

    public virtual T Get<T>()
    {
        return (T)Value;
    }

    public virtual Instance RealInstance => this;

    public virtual string Represent() => Value?.ToString() ?? "";
    public virtual string DebugRepresent() => $"{{{Type} {Represent()} {{{(Members != null ? string.Join(',', Members.Select(p => $"{p.Key} = {p.Value}")) : "")}}}}}";
    public override string ToString()
    {
        return Represent();
    }
}
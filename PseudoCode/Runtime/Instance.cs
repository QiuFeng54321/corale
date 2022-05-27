using PseudoCode.Runtime.Operations;

namespace PseudoCode.Runtime;

public class Instance
{
    public static Instance Null;
    public uint InstanceAddress;
    public Scope ParentScope;
    public PseudoProgram Program;

    public Instance(Scope parentScope, PseudoProgram program)
    {
        ParentScope = parentScope;
        Program = program;
    }

    public virtual Type Type { get; set; }
    public virtual Dictionary<string, Instance> Members { get; init; } = new();
    public virtual object Value { get; set; } = null!;

    public virtual Instance RealInstance => this;

    public virtual T Get<T>()
    {
        return (T)Value;
    }

    public virtual string Represent()
    {
        return Value?.ToString() ?? "";
    }

    public virtual string DebugRepresent()
    {
        return
            $"{{{Type} {Represent()} {{{(Members != null ? string.Join(',', Members.Select(p => $"{p.Key} = {p.Value}")) : "")}}}}}";
    }

    public override string ToString()
    {
        return Represent();
    }
}
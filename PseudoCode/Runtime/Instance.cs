namespace PseudoCode.Runtime;

public class Instance
{
    public virtual Type Type { get; set; }
    public virtual Dictionary<string, Instance> Members { get; init; } = new();
    public virtual object Value { get; set; } = null!;

    public virtual T Get<T>() => (T)Value;

    public override string ToString()
    {
        return
            $"{{{Type} {Value ?? ""} {{{(Members != null ? string.Join(',', Members.Select(p => $"{p.Key} = {p.Value}")) : "")}}}}}";
    }
}
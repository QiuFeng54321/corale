namespace PseudoCode.Runtime.Operations;

public class Operation
{
    public Scope ParentScope;
    public PseudoProgram Program;
    public SourceLocation SourceLocation;

    public Operation(Scope parentScope, PseudoProgram program)
    {
        ParentScope = parentScope;
        Program = program;
    }

    public virtual void Operate()
    {
    }

    public string Indent(int depth) => new(' ', depth * 4);
    public virtual string ToPlainString() => "Operation";
    public virtual string ToString(int depth)
    {
        return $"{Indent(depth)}{ToPlainString()} {(SourceLocation != null ? $"# {SourceLocation}" : "")}";
    }

    public override string ToString() => ToString(0);
}
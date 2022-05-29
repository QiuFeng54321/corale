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
        if (Program.DisplayOperations)
            Console.WriteLine(strings.Operation_Operate, ToPlainString(), SourceLocation);
    }

    public static string Indent(int depth)
    {
        return new string(' ', depth * 4);
    }

    public virtual string ToPlainString()
    {
        return strings.Operation_ToPlainString;
    }

    public virtual string ToString(int depth)
    {
        return $"{Indent(depth)}{ToPlainString()} {(SourceLocation != null ? $"# {SourceLocation}" : "")}";
    }

    public override string ToString()
    {
        return ToString(0);
    }
}
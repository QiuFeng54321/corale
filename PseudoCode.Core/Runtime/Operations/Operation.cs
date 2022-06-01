using Error = PseudoCode.Core.Runtime.Errors.Error;

namespace PseudoCode.Core.Runtime.Operations;

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
        if (Program.DisplayOperationsAtRuntime)
            Console.WriteLine(strings.Operation_Operate, ToPlainString(), SourceLocation);
    }

    public void HandledOperate()
    {
        try
        {
            Operate();
        }
        catch (Errors.Error e)
        {
            e.Operation ??= this;
            // e.OperationStackTrace.Add(this);
            throw;
        }
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
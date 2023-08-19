using PseudoCode.Core.Runtime.Errors;

namespace PseudoCode.Core.Runtime.Operations;

public class Operation
{
    public readonly PseudoProgram Program;
    public Scope ParentScope;
    public SourceLocation PoiLocation; // Point of interest, position of operator, etc.
    public SourceRange SourceRange;

    public Operation(Scope parentScope, PseudoProgram program)
    {
        ParentScope = parentScope;
        Program = program;
    }

    public virtual void Operate()
    {
        if (Program.DisplayOperationsAtRuntime)
            Console.WriteLine(strings.Operation_Operate, ToPlainString(), PoiLocation);
    }

    public virtual void MetaOperate()
    {
    }

    public void HandledOperate()
    {
        try
        {
            Operate();
        }
        catch (Error e)
        {
            e.Operation ??= this;
            // e.OperationStackTrace.Add(this);
            throw;
        }
        catch (Exception e)
        {
            throw new UnhandledError(e, this);
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
        return $"{Indent(depth)}{ToPlainString()} {(PoiLocation != null ? $"# {PoiLocation}" : "")}";
    }

    public override string ToString()
    {
        return ToString(0);
    }
}
using Type = PseudoCode.Core.Runtime.Types.Type;

namespace PseudoCode.Core.Runtime.Operations;

public class CaseOperation : Operation
{
    public List<(Scope Condition, Scope Operation)> Cases = new();
    public Scope DefaultCase;
    public DuplicateOperation DuplicateOperation;

    public CaseOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        var fallback = true;
        foreach (var (condition, operation) in Cases)
        {
            condition.Operate();
            var test = Program.RuntimeStack.Pop();
            if (!Program.FindTypeDefinition(Type.BooleanId).Type.CastFrom(test).Get<bool>()) continue;
            operation.Operate();
            fallback = false;
            break;
        }

        if (fallback)
        {
            DefaultCase.Operate();
        }

        Program.RuntimeStack.Pop();
    }

    public override void MetaOperate()
    {
        base.MetaOperate();
        foreach (var (condition, operation)in Cases)
        {
            condition.MetaOperate();
            Program.TypeCheckStack.Pop();
            operation.MetaOperate();
        }

        DefaultCase?.MetaOperate();
        Program.TypeCheckStack.Pop();
    }

    public override string ToPlainString() => "Case";

    public override string ToString(int depth)
    {
        return
            $"{Indent(depth)}Case:{string.Join($"\n", Cases.Select(c => $"{c.Condition.ToString(depth)}\n{Indent(depth)}THEN:\n{c.Operation.ToString(depth)}"))}{(DefaultCase == null ? "" : $"\n{Indent(depth)}OTHERWISE: {DefaultCase.ToString(depth)}")}";
    }
}
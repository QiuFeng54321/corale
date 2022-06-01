namespace PseudoCode.Core.Runtime.Operations;

public class AssignmentOperation : Operation
{
    public bool KeepVariableInStack;
    public AssignmentOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        var value = Program.RuntimeStack.Pop();
        var to = Program.RuntimeStack.Pop();
        var res = to.Type.Assign(to, value);
        if (KeepVariableInStack) Program.RuntimeStack.Push(res);
    }

    public override void MetaOperate()
    {
        base.MetaOperate();
        var value = Program.TypeCheckStack.Pop();
        var to = Program.TypeCheckStack.Pop();
        if (to is PlaceholderType placeholderType) to = placeholderType.MetaAssign(value);
        // TODO: Type check
        if (KeepVariableInStack) Program.TypeCheckStack.Push(to);
    }

    public override string ToPlainString()
    {
        return string.Format(strings.AssignmentOperation_ToPlainString, KeepVariableInStack ? strings.AssignmentOperation_ToPlainString_Keep : "");
    }
}
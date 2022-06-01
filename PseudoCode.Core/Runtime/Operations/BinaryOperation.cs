namespace PseudoCode.Core.Runtime.Operations;

public class BinaryOperation : Operation
{
    public int OperatorMethod;

    public BinaryOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        var value = Program.RuntimeStack.Pop();
        var to = Program.RuntimeStack.Pop();
        to = to.Type.BinaryOperators[OperatorMethod](to, value);
        Program.RuntimeStack.Push(to);
    }

    public override void MetaOperate()
    {
        base.MetaOperate();
        var value = Program.TypeCheckStack.Pop();
        var to = Program.TypeCheckStack.Pop();
        to = to.BinaryResultType(OperatorMethod, value);
        Program.TypeCheckStack.Push(to);
    }

    public override string ToPlainString()
    {
        return string.Format(strings.BinaryOperation_ToPlainString, OperatorMethod);
    }
}
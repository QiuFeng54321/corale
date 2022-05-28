namespace PseudoCode.Runtime.Operations;

public class ArrayIndexOperation : Operation
{
    public ArrayIndexOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        var index = (ArrayInstance)Program.RuntimeStack.Pop();
        var instance = Program.RuntimeStack.Pop();
        Program.RuntimeStack.Push(instance.Type.Index(instance, index));
    }

    public override string ToPlainString()
    {
        return "Array index access";
    }
}
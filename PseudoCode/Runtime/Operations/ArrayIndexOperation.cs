namespace PseudoCode.Runtime.Operations;

public class ArrayIndexOperation : Operation
{
    public ArrayIndexOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        var index = (ArrayInstance)ParentScope.RuntimeStack.Pop();
        var instance = ParentScope.RuntimeStack.Pop();
        ParentScope.RuntimeStack.Push(instance.Type.Index(instance, index));
    }

    public override string ToPlainString() => "Array index access";
}
namespace PseudoCode.Runtime.Operations;

public class InputOperation : Operation
{

    public InputOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        var instance = ParentScope.RuntimeStack.Pop();
        var input = Console.ReadLine();
        instance.Type.Assign(instance, ParentScope.FindType("STRING").Instance(input));
    }

    public override string ToPlainString()
    {
        return $"Input";
    }
}
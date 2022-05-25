using System.Collections;

namespace PseudoCode.Runtime.Operations;

public class OutputOperation : Operation
{
    public int ArgumentCount;
    public override void Operate()
    {
        base.Operate();
        List<Instance> arguments = new();
        for(var i = 0; i < ArgumentCount; i++)
            arguments.Insert(0, ParentScope.RuntimeStack.Pop());
        Console.WriteLine(string.Join(' ', arguments));
    }

    public override string ToPlainString()
    {
        return $"Output {ArgumentCount}";
    }

    public OutputOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }
}
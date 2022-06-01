namespace PseudoCode.Core.Runtime.Operations;

public class OutputOperation : Operation
{
    public int ArgumentCount;

    public OutputOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        List<Instance> arguments = new();
        for (var i = 0; i < ArgumentCount; i++)
            arguments.Insert(0, Program.RuntimeStack.Pop());
        Console.WriteLine(string.Join(' ', arguments));
    }

    public override void MetaOperate()
    {
        base.MetaOperate();
        for (var i = 0; i < ArgumentCount; i++)
            Program.TypeCheckStack.Pop();
    }

    public override string ToPlainString()
    {
        return string.Format(strings.OutputOperation_ToPlainString, ArgumentCount);
    }
}
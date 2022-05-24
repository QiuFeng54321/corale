namespace PseudoCode.Runtime.Operations;

public class OutputOperation : Operation
{
    public override void Operate()
    {
        base.Operate();
        var value = Scope.RuntimeStack.Pop();
        Console.WriteLine(value);
    }

    public override string ToString()
    {
        return $"Output";
    }
}
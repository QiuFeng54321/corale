namespace PseudoCode.Runtime.Operations;

public class IfOperation : Operation
{
    public Operation TrueBlock, FalseBlock;

    public override void Operate()
    {
        base.Operate();
        var test = Scope.FindType("BOOLEAN").CastFrom(Scope.RuntimeStack.Pop());
        if (test.Get<bool>())
        {
            TrueBlock.Operate();
        }
        else
        {
            FalseBlock?.Operate();
        }
    }

    public override string ToString()
    {
        return $"If:\n{TrueBlock}\n{(FalseBlock != null ? $"Else:\n{FalseBlock}" : "")}";
    }
}
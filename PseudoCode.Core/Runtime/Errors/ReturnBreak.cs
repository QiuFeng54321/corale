namespace PseudoCode.Core.Runtime.Errors;

public class ReturnBreak : Error
{
    public ReturnBreak(string message, IEnumerable<string> possibleCauses = default) : base(message, possibleCauses)
    {
    }

    public override string Name => "ReturnBreak";
}
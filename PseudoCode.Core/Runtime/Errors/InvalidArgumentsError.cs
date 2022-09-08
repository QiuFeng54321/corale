namespace PseudoCode.Core.Runtime.Errors;

public class InvalidArgumentsError : Error
{
    public InvalidArgumentsError(string message, IEnumerable<string> possibleCauses = default) : base(message,
        possibleCauses)
    {
    }

    public override string Name => "InvalidArgumentsError";
}
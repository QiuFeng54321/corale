namespace PseudoCode.Core.Runtime.Errors;

public class InvalidAccessError : Error
{
    public InvalidAccessError(string message, IEnumerable<string> possibleCauses = default) : base(message,
        possibleCauses)
    {
    }

    public override string Name => strings.InvalidAccessError_Name;
}
namespace PseudoCode.Core.Runtime.Errors;

public class InvalidTypeError : Error
{
    public InvalidTypeError(string message, IEnumerable<string> possibleCauses = default) : base(message,
        possibleCauses)
    {
    }

    public override string Name => strings.InvalidTypeError_Name;
}
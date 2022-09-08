namespace PseudoCode.Core.Runtime.Errors;

public class OutOfBoundsError : Error
{
    public OutOfBoundsError(string message, IEnumerable<string> possibleCauses = default) : base(message,
        possibleCauses)
    {
    }

    public override string Name => strings.OutOfBoundsError_Name;
}
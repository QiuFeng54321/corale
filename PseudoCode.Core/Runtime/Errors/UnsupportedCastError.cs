namespace PseudoCode.Core.Runtime.Errors;

public class UnsupportedCastError : Error
{
    public UnsupportedCastError(string message, IEnumerable<string> possibleCauses = default) : base(message,
        possibleCauses)
    {
    }

    public override string Name => strings.UnsupportedCastError_Name;
}
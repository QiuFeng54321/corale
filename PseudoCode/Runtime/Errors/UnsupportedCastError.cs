using PseudoCode.Runtime.Operations;

namespace PseudoCode.Runtime.Errors;

public class UnsupportedCastError : Error
{
    public override string Name => strings.UnsupportedCastError_Name;

    public UnsupportedCastError(string message, Operation operation, IEnumerable<string> possibleCauses = default,
        List<Operation> stackTrace = default) : base(message, operation, possibleCauses, stackTrace)
    {
    }
}
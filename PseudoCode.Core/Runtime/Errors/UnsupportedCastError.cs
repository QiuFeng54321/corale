using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime.Errors;

public class UnsupportedCastError : Error
{
    public UnsupportedCastError(string message, Operation operation, IEnumerable<string> possibleCauses = default,
        List<Operation> stackTrace = default) : base(message, operation, possibleCauses, stackTrace)
    {
    }

    public override string Name => strings.UnsupportedCastError_Name;
}
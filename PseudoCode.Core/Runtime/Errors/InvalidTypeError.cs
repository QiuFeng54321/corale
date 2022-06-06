using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime.Errors;

public class InvalidTypeError : Error
{
    public InvalidTypeError(string message, Operation operation, IEnumerable<string> possibleCauses = default,
        List<Operation> stackTrace = default) : base(message, operation, possibleCauses, stackTrace)
    {
    }

    public override string Name => strings.InvalidTypeError_Name;
}
using PseudoCode.Runtime.Operations;

namespace PseudoCode.Runtime.Errors;

public class InvalidTypeError : Error
{
    public InvalidTypeError(string message, Operation operation, IEnumerable<string> possibleCauses = default, List<Operation> stackTrace = default) : base(message, operation, possibleCauses, stackTrace)
    {
    }
}
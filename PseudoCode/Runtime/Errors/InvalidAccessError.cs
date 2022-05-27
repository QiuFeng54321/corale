using PseudoCode.Runtime.Operations;

namespace PseudoCode.Runtime.Errors;

public class InvalidAccessError : Error
{
    public InvalidAccessError(string message, Operation operation, IEnumerable<string> possibleCauses = default, List<Operation> stackTrace = default) : base(message, operation, possibleCauses, stackTrace)
    {
    }
}
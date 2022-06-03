using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime.Errors;

public class InvalidArgumentsError : Error
{
    public override string Name => "InvalidArgumentsError";

    public InvalidArgumentsError(string message, Operation operation, IEnumerable<string> possibleCauses = default, List<Operation> stackTrace = default) : base(message, operation, possibleCauses, stackTrace)
    {
    }
}
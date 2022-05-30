using PseudoCode.Runtime.Operations;

namespace PseudoCode.Runtime.Errors;

public class OutOfBoundsError : Error
{
    public override string Name => strings.OutOfBoundsError_Name;

    public OutOfBoundsError(string message, Operation operation, IEnumerable<string> possibleCauses = default, List<Operation> stackTrace = default) : base(message, operation, possibleCauses, stackTrace)
    {
    }
}
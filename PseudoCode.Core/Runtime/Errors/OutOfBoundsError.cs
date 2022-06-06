using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime.Errors;

public class OutOfBoundsError : Error
{
    public OutOfBoundsError(string message, Operation operation, IEnumerable<string> possibleCauses = default,
        List<Operation> stackTrace = default) : base(message, operation, possibleCauses, stackTrace)
    {
    }

    public override string Name => strings.OutOfBoundsError_Name;
}
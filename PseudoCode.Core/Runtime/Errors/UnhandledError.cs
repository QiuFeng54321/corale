using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime.Errors;

public class UnhandledError : Error
{
    public override string Name => "UnhandledError";
    public Exception Exception;

    public UnhandledError(Exception exception, Operation operation, IEnumerable<string> possibleCauses = default, List<Operation> stackTrace = default) : base(exception.ToString(), operation, possibleCauses, stackTrace)
    {
        Exception = exception;
    }
}
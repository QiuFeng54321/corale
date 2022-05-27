using PseudoCode.Runtime.Operations;

namespace PseudoCode.Runtime.Errors;

public class Error : Exception
{
    public List<Operation> OperationStackTrace;
    public IEnumerable<string> PossibleCauses;
    public Operation Operation;

    public Error(string message, Operation operation, IEnumerable<string> possibleCauses = default,
        List<Operation> stackTrace = default) : base(message)
    {
        OperationStackTrace = stackTrace ?? new List<Operation>();
        PossibleCauses = possibleCauses ?? new List<string>();
        Operation = operation;
    }

    public override string ToString()
    {
        return $@"
{GetType().Name}: {Message}
Occured in operation: {Operation}.
Stack Trace:
{string.Join('\n', OperationStackTrace.Select(o => o.ToPlainString()))}
Possible Causes:
{string.Join('\n', PossibleCauses)}";
    }
}
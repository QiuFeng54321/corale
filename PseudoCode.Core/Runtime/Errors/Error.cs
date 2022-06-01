using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime.Errors;

public class Error : Exception
{
    public virtual string Name => strings.Error_Name;
    public Operation Operation;
    public List<Operation> OperationStackTrace;
    public IEnumerable<string> PossibleCauses;

    public Error(string message, Operation operation, IEnumerable<string> possibleCauses = default,
        List<Operation> stackTrace = default) : base(message)
    {
        OperationStackTrace = stackTrace ?? new List<Operation>();
        PossibleCauses = possibleCauses ?? new List<string>();
        Operation = operation;
    }

    public override string ToString()
    {
        return string.Format(strings.Error_ToString, Name, Message, Operation,
            string.Join('\n', OperationStackTrace.Select(o => $"{o.ToPlainString()} {o.PoiLocation}")),
            string.Join('\n', PossibleCauses));
    }
}
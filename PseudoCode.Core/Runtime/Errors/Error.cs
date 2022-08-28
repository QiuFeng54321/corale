using PseudoCode.Core.Runtime.Emit.Opcodes;
using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime.Errors;

public class Error : Exception
{
    public IOpcode Operation;
    public List<IOpcode> OperationStackTrace;
    public IEnumerable<string> PossibleCauses;

    public Error(string message, IOpcode operation, IEnumerable<string> possibleCauses = default,
        List<IOpcode> stackTrace = default) : base(message)
    {
        OperationStackTrace = stackTrace ?? new List<IOpcode>();
        PossibleCauses = possibleCauses ?? new List<string>();
        Operation = operation;
    }

    public virtual string Name => strings.Error_Name;

    public override string ToString()
    {
        return string.Format(strings.Error_ToString, Name, Message, Operation,
            string.Join('\n', OperationStackTrace.Select(o => $"{o.ToPlainString()} {o.PoiLocation}")),
            string.Join('\n', PossibleCauses));
    }
}
namespace PseudoCode.Core.Runtime.Errors;

public class UnhandledError : Error
{
    public Exception Exception;

    public UnhandledError(Exception exception, IEnumerable<string> possibleCauses = default) : base(
        exception.ToString(), possibleCauses)
    {
        Exception = exception;
    }

    public override string Name => "UnhandledError";
}
namespace PseudoCode.Core.Runtime.Errors;

public class FileError : Error
{
    public FileError(string message, IEnumerable<string> possibleCauses = default) : base(message, possibleCauses)
    {
    }

    public override string Name => "FileError";
}
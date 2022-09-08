namespace PseudoCode.Core.Runtime.Errors;

public class Error : Exception
{
    public IEnumerable<string> PossibleCauses;

    public Error(string message, IEnumerable<string> possibleCauses = default) : base(message)
    {
        PossibleCauses = possibleCauses ?? new List<string>();
    }

    public virtual string Name => strings.Error_Name;

    public override string ToString()
    {
        return string.Format(strings.Error_ToString, Name, Message, null,
            "",
            string.Join('\n', PossibleCauses));
    }
}
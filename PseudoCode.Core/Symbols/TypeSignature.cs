namespace PseudoCode.Core.Symbols;

public class TypeSignature : Signature
{
    public Signature[] Members;

    public override string Represent()
    {
        return $"T{Members.Length}{string.Join("", Members.Select(x => x.ToString()))}";
    }
}
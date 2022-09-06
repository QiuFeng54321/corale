namespace PseudoCode.Core.Symbols;

public class FunctionSignature : Signature
{
    public Signature[] Arguments;
    public Signature ReturnSignature;

    public override string Represent()
    {
        return $"F{ReturnSignature}{Arguments.Length}{string.Join("", Arguments.Select(x => x.ToString()))}";
    }
}
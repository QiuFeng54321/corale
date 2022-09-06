namespace PseudoCode.Core.Symbols;

public class PointerSignature : Signature
{
    public Signature Signature;

    public override string Represent()
    {
        return $"P{Signature}";
    }
}
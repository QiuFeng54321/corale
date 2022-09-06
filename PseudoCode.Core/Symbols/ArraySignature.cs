namespace PseudoCode.Core.Symbols;

/// <summary>
/// </summary>
public class ArraySignature : Signature
{
    public int Dimensions;
    public Signature ElementSignature;

    public override string Represent()
    {
        return $"A{Dimensions}{ElementSignature}";
    }
}
namespace PseudoCode.Core.Symbols;

public class PrimitiveSignature : Signature
{
    public static readonly PrimitiveSignature Integer = new('I'),
        Real = new('R'),
        Boolean = new('B'),
        Date = new('D'),
        String = new('S'),
        Char = new('C'),
        Void = new('V');

    public char Character;

    public PrimitiveSignature(char c)
    {
        Character = c;
    }

    public override string Represent()
    {
        return Character.ToString();
    }
}
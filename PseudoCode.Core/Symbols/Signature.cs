namespace PseudoCode.Core.Symbols;

/// <summary>
///     Type signatures. Should support these representations:
///     None: N
///     Primitive:
///     I: Integer
///     R: Real
///     C: Char
///     S: String
///     D: Date (?)
///     B: Boolean
///     V: Void
///     Array:
///     A3I: Array of integer (3d)
///     Function:
///     FV3ISR: Function(Integer, String, Real) Returns Void
///     Pointer:
///     PI: Integer Pointer
///     Type:
///     T5SIIRB: TYPE {String, Integer, Integer, Real, Bool}
///     Enum is Integer (Temp)
///     Flags: d: Direct Reference, c: Constant, i: Immutable, a: Assignable. They are in alphabetical order
///     Compound:
///     adPFB4A2IdIPIPdiT2II: Pointer of Function(Array[Dim 2] Of Integer, BYREF Integer, Pointer of Integer, Pointer of
///     REF IMMUTABLE Type {Integer, Integer}) Returns Boolean
/// </summary>
public class Signature
{
    public SignatureFlags Flags;

    public string FormatFlags()
    {
        var s = "";
        if (Flags.HasFlag(SignatureFlags.Assignable)) s += 'a';
        if (Flags.HasFlag(SignatureFlags.Constant)) s += 'c';
        if (Flags.HasFlag(SignatureFlags.DirectReference)) s += 'd';
        if (Flags.HasFlag(SignatureFlags.Immutable)) s += 'i';
        return s;
    }

    public override string ToString()
    {
        return $"{FormatFlags()}{Represent()}";
    }

    public virtual string Represent()
    {
        throw new InvalidOperationException();
    }
}
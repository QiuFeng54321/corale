namespace PseudoCode.Core.Symbols;

[Flags]
public enum SignatureFlags
{
    None = 0,
    DirectReference = 1 << 0,
    Assignable = 1 << 1,
    Immutable = 1 << 2,
    Constant = 1 << 3
}
namespace PseudoCode.Core.Runtime.Emit;

public record Label(string Prefix, int Postfix, long OpcodeIndex = -1)
{
    public override string ToString() => $"{Prefix}{Postfix:0000}";
    public long OpcodeIndex { get; set; } = OpcodeIndex;
}
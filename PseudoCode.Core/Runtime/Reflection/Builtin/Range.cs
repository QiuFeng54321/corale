using System.Runtime.InteropServices;

namespace PseudoCode.Core.Runtime.Reflection.Builtin;

[StructLayout(LayoutKind.Sequential)]
[BuiltinType("RANGE")]
public struct Range
{
    public long Start, End;
    public long Length => End - Start + 1;

    public long ToRealIndex(long pseudoIndex)
    {
        if (pseudoIndex < 0) return End + pseudoIndex - Start;
        return pseudoIndex - Start;
    }

    public override string ToString()
    {
        return $"{Start}:{End}";
    }
}
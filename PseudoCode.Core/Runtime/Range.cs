namespace PseudoCode.Core.Runtime;

public class Range
{
    public int Start, End;
    public int Length => End - Start + 1;

    public int ToRealIndex(int pseudoIndex)
    {
        if (pseudoIndex < 0) return End + pseudoIndex - Start;
        return pseudoIndex - Start;
    }

    public override string ToString()
    {
        return $"{Start}:{End}";
    }
}
namespace PseudoCode.Runtime;

public class SourceLocation
{
    public int Line, Column;

    public SourceLocation(int line, int column)
    {
        Line = line;
        Column = column;
    }

    public override string ToString()
    {
        return $"({Line}:{Column})";
    }
}
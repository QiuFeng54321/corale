namespace PseudoCode.Core.Runtime;

public class SourceLocation
{
    public static readonly SourceLocation Identity = new(-1, -1);
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

    protected bool Equals(SourceLocation other)
    {
        return Line == other.Line && Column == other.Column;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((SourceLocation)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Line, Column);
    }

    public static bool operator ==(SourceLocation left, SourceLocation right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(SourceLocation left, SourceLocation right)
    {
        return !Equals(left, right);
    }

    public static bool operator <=(SourceLocation left, SourceLocation right)
    {
        return left.Line < right.Line || (left.Line == right.Line && left.Column <= right.Column);
    }

    public static bool operator >=(SourceLocation left, SourceLocation right)
    {
        return left.Line > right.Line || (left.Line == right.Line && left.Column >= right.Column);
    }

    public static bool operator <(SourceLocation left, SourceLocation right)
    {
        return left.Line < right.Line || (left.Line == right.Line && left.Column < right.Column);
    }

    public static bool operator >(SourceLocation left, SourceLocation right)
    {
        return left.Line > right.Line || (left.Line == right.Line && left.Column > right.Column);
    }
}
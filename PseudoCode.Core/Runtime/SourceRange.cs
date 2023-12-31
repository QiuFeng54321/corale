namespace PseudoCode.Core.Runtime;

public class SourceRange
{
    public static readonly SourceRange Identity = new(SourceLocation.Identity, SourceLocation.Identity);

    public SourceRange(SourceLocation start, SourceLocation end)
    {
        Start = start;
        End = end;
    }

    public SourceLocation Start { get; }
    public SourceLocation End { get; }

    public bool Contains(SourceLocation location)
    {
        return Start <= location && End >= location;
    }

    protected bool Equals(SourceRange other)
    {
        return Equals(Start, other.Start) && Equals(End, other.End);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((SourceRange)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Start, End);
    }

    public static bool operator ==(SourceRange left, SourceRange right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(SourceRange left, SourceRange right)
    {
        return !Equals(left, right);
    }

    public override string ToString()
    {
        return $"{Start} to {End}";
    }

    public void Deconstruct(out SourceLocation Start, out SourceLocation End)
    {
        Start = this.Start;
        End = this.End;
    }
}
namespace PseudoCode.Core.Runtime;

public class SourceRange
{
    public static readonly SourceRange Identity = new(SourceLocation.Identity, SourceLocation.Identity);

    public SourceRange(SourceLocation Start, SourceLocation End)
    {
        this.Start = Start;
        this.End = End;
    }

    public SourceLocation Start { get; set; }
    public SourceLocation End { get; set; }

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
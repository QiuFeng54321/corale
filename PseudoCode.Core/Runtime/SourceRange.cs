namespace PseudoCode.Core.Runtime;

public class SourceRange
{
    public SourceRange(SourceLocation Start, SourceLocation End)
    {
        this.Start = Start;
        this.End = End;
    }

    public bool Contains(SourceLocation location) => Start <= location && End >= location;
    public SourceLocation Start { get; set; }
    public SourceLocation End { get; set; }
    public static SourceRange Identity = new(SourceLocation.Identity, SourceLocation.Identity);

    public override string ToString()
    {
        return $"{Start} to {End}";
    }

    public void Deconstruct(out SourceLocation Start, out SourceLocation End)
    {
        Start = this.Start;
        End = this.End;
    }
};
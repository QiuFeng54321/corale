namespace PseudoCode.Core.CodeGen;

public abstract class AstNode
{
    public abstract string Format();

    public override string ToString()
    {
        return Format();
    }
}
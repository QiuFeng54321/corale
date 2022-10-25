namespace PseudoCode.Core.CodeGen;

public class GenericDeclaration : AstNode
{
    public readonly List<string> Identifiers;

    public GenericDeclaration(List<string> identifiers)
    {
        Identifiers = identifiers;
    }

    public override string ToString()
    {
        return $"<{string.Join(", ", Identifiers)}>";
    }
}
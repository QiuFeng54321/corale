namespace PseudoCode.Core.CodeGen;

public class GenericDeclaration : AstNode
{
    public List<string> Identifiers;

    public GenericDeclaration(List<string> identifiers)
    {
        Identifiers = identifiers;
    }

    public override string ToString()
    {
        return $"<{string.Join(", ", Identifiers)}>";
    }
}
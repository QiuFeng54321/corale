using PseudoCode.Core.Formatting;

namespace PseudoCode.Core.CodeGen;

public class TypeDeclaration : Statement
{
    public readonly List<DeclarationStatement> DeclarationStatements;
    public readonly GenericDeclaration GenericDeclaration;
    public readonly string Name;

    public TypeDeclaration(string name, GenericDeclaration genericDeclaration = default,
        List<DeclarationStatement> declarationStatements = default)
    {
        Name = name;
        GenericDeclaration = genericDeclaration;
        DeclarationStatements = declarationStatements;
    }

    public override void CodeGen(CodeGenContext ctx, Block block)
    {
        // throw new NotImplementedException();
    }

    public override void Format(PseudoFormatter formatter)
    {
        formatter.WriteStatement($"TYPE {Name}{GenericDeclaration}");
        if (DeclarationStatements != null)
        {
            formatter.Indent();
            foreach (var declarationStatement in DeclarationStatements) declarationStatement.Format(formatter);

            formatter.Dedent();
        }

        formatter.WriteStatement("ENDTYPE");
    }
}
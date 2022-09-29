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
        var resType = new Type
        {
            TypeName = Name,
            Kind = Types.Type,
            IsGeneric = GenericDeclaration != null && GenericDeclaration.Identifiers.Count != 0,
            GenericArguments = GenericDeclaration?.Identifiers.Select(Symbol.MakeGenericPlaceholderSymbol).ToList(),
            GenericMembers = new List<Symbol>()
        };
        block.Namespace.AddSymbol(Symbol.MakeTypeSymbol(resType));
        Dictionary<string, Symbol> typeMembers = new();
        foreach (var declarationStatement in DeclarationStatements)
        {
            var typeSym = declarationStatement.GetTypeSymbol(ctx, block, resType);
            var sym = declarationStatement.MakeSymbol(typeSym);
            typeMembers.Add(declarationStatement.Name, sym);
            resType.GenericMembers.Add(sym);
        }

        resType.Members = typeMembers;

        // resType.GetLLVMType().Dump();

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
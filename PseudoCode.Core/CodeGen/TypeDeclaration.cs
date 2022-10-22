using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.Formatting;

namespace PseudoCode.Core.CodeGen;

/// <summary>
///     This is for type declaration, but will be used to make classes too
/// </summary>
public class TypeDeclaration : Statement, IGenericExpression
{
    public readonly List<DeclarationStatement> DeclarationStatements;
    public readonly GenericDeclaration GenericDeclaration;
    public readonly string TypeName;

    public TypeDeclaration(string typeName, GenericDeclaration genericDeclaration = default,
        List<DeclarationStatement> declarationStatements = default)
    {
        TypeName = typeName;
        GenericDeclaration = genericDeclaration;
        DeclarationStatements = declarationStatements;
    }

    public Symbol Generate(CodeGenContext ctx, Function function, List<Symbol> genericParams = default)
    {
        var typeName = Type.GenerateFilledGenericTypeName(TypeName, genericParams);
        if (function.BodyNamespace.TryGetSymbol(typeName, out var existingSymbol)) return existingSymbol;
        // 创建一个悬挂块，不会改动Statements但是能达到独立查找的效果
        var subNs = function.BodyNamespace.AddNamespace(typeName);
        if (genericParams != null)
            for (var i = 0; i < genericParams.Count; i++)
                subNs.AddSymbol(genericParams[i], false, GenericDeclaration.Identifiers[i]);

        var resType = new Type
        {
            TypeName = typeName,
            Kind = Types.Type
        };
        var typeSymbol = Symbol.MakeTypeSymbol(resType);
        function.BodyNamespace.AddSymbol(typeSymbol);
        Dictionary<string, Symbol> typeMembers = new();
        for (var index = 0; index < DeclarationStatements.Count; index++)
        {
            var declarationStatement = DeclarationStatements[index];
            var typeSym = declarationStatement.GetTypeSymbol(ctx, function, subNs);
            var sym = declarationStatement.MakeSymbol(typeSym);
            typeMembers.Add(declarationStatement.Name, sym);
            sym.TypeMemberIndex = index;
        }

        resType.Members = typeMembers;
        return typeSymbol;
    }

    public override void CodeGen(CodeGenContext ctx, Function function)
    {
        Generate(ctx, function);
    }

    public override void Format(PseudoFormatter formatter)
    {
        formatter.WriteStatement($"TYPE {TypeName}{GenericDeclaration}");
        if (DeclarationStatements != null)
        {
            formatter.Indent();
            foreach (var declarationStatement in DeclarationStatements) declarationStatement.Format(formatter);

            formatter.Dedent();
        }

        formatter.WriteStatement("ENDTYPE");
    }
}
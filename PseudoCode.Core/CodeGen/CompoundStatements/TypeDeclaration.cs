using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.CodeGen.Expressions;
using PseudoCode.Core.CodeGen.SimpleStatements;
using PseudoCode.Core.Formatting;

namespace PseudoCode.Core.CodeGen.CompoundStatements;

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

    public Symbol Generate(CodeGenContext ctx, CompilationUnit cu, Function function,
        List<Symbol> genericParams = default)
    {
        var typeName = Type.GenerateFilledGenericTypeName(TypeName, genericParams);
        if (function.BodyNamespace.TryGetSymbol(typeName, out var existingSymbol)) return existingSymbol;
        // 创建一个悬挂块，不会改动Statements但是能达到独立查找的效果
        var subNs = function.BodyNamespace.AddNamespace(typeName);
        if (genericParams != null)
            for (var i = 0; i < genericParams.Count; i++)
                subNs.AddSymbol(genericParams[i], false, GenericDeclaration.Identifiers[i]);

        List<Symbol> typeMembers = new();
        for (var index = 0; index < DeclarationStatements.Count; index++)
        {
            var declarationStatement = DeclarationStatements[index];
            var typeSym = declarationStatement.GetTypeSymbol(ctx, cu, function, subNs);
            var sym = typeSym.MakeStructMemberDeclSymbol(declarationStatement.Name);
            typeMembers.Add(sym);
            sym.TypeMemberIndex = index;
        }

        var record = new Record
        {
            TypeName = typeName,
            Members = typeMembers
        };
        record.CodeGen(ctx, cu, function);
        return record.TypeSymbol;
    }

    public override void CodeGen(CodeGenContext ctx, CompilationUnit cu, Function function)
    {
        Generate(ctx, cu, function);
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
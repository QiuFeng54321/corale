using PseudoCode.Core.Formatting;

namespace PseudoCode.Core.CodeGen.Containers;

public class Record : Statement
{
    public List<Symbol> Members;
    public string TypeName;
    public Symbol TypeSymbol;

    public override void Format(PseudoFormatter formatter)
    {
        throw new InvalidOperationException();
    }

    public override void CodeGen(CodeGenContext ctx, CompilationUnit cu, Function function)
    {
        var resType = new Type
        {
            TypeName = TypeName,
            Kind = Types.Type,
            Members = Members
        };
        TypeSymbol = Symbol.MakeTypeSymbol(resType, function.BodyNamespace);
        function.BodyNamespace.AddSymbol(TypeSymbol);
    }
}
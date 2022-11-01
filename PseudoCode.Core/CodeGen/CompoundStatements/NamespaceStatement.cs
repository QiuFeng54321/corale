using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.CodeGen.TypeLookups;
using PseudoCode.Core.Formatting;

namespace PseudoCode.Core.CodeGen.CompoundStatements;

public class NamespaceStatement : Statement
{
    public Block Block;
    public NamespaceLookup NamespaceLookup;

    public override void Format(PseudoFormatter formatter)
    {
        formatter.WriteStatement($"NAMESPACE {NamespaceLookup}");
        if (Block != null)
        {
            Block.Format(formatter);
            formatter.WriteStatement("ENDNAMESPACE");
        }
    }

    public override void CodeGen(CodeGenContext ctx, CompilationUnit cu, Function function)
    {
        var nsBefore = function.BodyNamespace;
        function.BodyNamespace = NamespaceLookup.GenerateNs(ctx);
        if (Block != null)
        {
            Block.CodeGen(ctx, cu, function);
            function.BodyNamespace = nsBefore;
        }
    }
}
using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.CodeGen.TypeLookups;
using PseudoCode.Core.Formatting;

namespace PseudoCode.Core.CodeGen.SimpleStatements;

public class UseNamespaceStatement : Statement
{
    public NamespaceLookup NamespaceLookup;

    public override void Format(PseudoFormatter formatter)
    {
        formatter.WriteStatement($"USE NAMESPACE {NamespaceLookup}");
    }

    public override void CodeGen(CodeGenContext ctx, CompilationUnit cu, Function function)
    {
        var ns = NamespaceLookup.LookupNs(ctx, function);
        function.BodyNamespace.AddNamespaceAlias(ns);
    }
}
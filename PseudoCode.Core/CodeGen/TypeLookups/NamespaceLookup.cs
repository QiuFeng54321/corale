using PseudoCode.Core.Analyzing;
using PseudoCode.Core.CodeGen.Containers;

namespace PseudoCode.Core.CodeGen.TypeLookups;

public class NamespaceLookup : AstNode
{
    public readonly string Identifier;
    public readonly NamespaceLookup ParentNs;

    public NamespaceLookup(string identifier, NamespaceLookup parentNs = null)
    {
        Identifier = identifier;
        ParentNs = parentNs;
    }

    public Namespace LookupNs(CodeGenContext ctx, Function function)
    {
        if (ParentNs is null)
        {
            if (function.BodyNamespace.TryGetNamespace(Identifier, out var rtNs)) return rtNs;
            return null;
        }

        var parentNs = ParentNs.LookupNs(ctx, function);
        if (parentNs == null) return null;
        if (parentNs.TryGetNamespace(Identifier, out var ns)) return ns;
        return null;
    }

    public Symbol Lookup(CodeGenContext ctx, Function function)
    {
        var ns = ParentNs?.LookupNs(ctx, function) ?? function.BodyNamespace;
        if (ns == null)
        {
            ctx.Analysis.Feedbacks.Add(new Feedback
            {
                Message = $"Unknown namespace: {ToString()}",
                Severity = Feedback.SeverityType.Error,
                DebugInformation = DebugInformation
            });
            return Symbol.ErrorSymbol;
        }

        if (ns.TryGetSymbol(Identifier, out var sym)) return sym;
        ctx.Analysis.Feedbacks.Add(new Feedback
        {
            Message = $"Unknown symbol: {ToString()}",
            Severity = Feedback.SeverityType.Error,
            DebugInformation = DebugInformation
        });
        return Symbol.ErrorSymbol;
    }

    public override string ToString()
    {
        return ParentNs == null ? Identifier : $"{ParentNs}::{Identifier}";
    }
}
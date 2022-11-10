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

    public Namespace LookupNs(CodeGenContext ctx, Function function, Namespace ns)
    {
        if (ParentNs is null)
        {
            if (ns.TryGetNamespace(Identifier, out var rtNs)) return rtNs;
            return null;
        }

        var parentNs = ParentNs.LookupNs(ctx, function, ns);
        if (parentNs == null) return null;
        if (parentNs.TryGetNamespace(Identifier, out var nsres)) return nsres;
        return null;
    }

    public Symbol Lookup(CodeGenContext ctx, Function function, Namespace ns)
    {
        var parentNs = ParentNs?.LookupNs(ctx, function, ns) ?? ns ?? function.BodyNamespace;
        if (parentNs == null)
        {
            ctx.Analysis.Feedbacks.Add(new Feedback
            {
                Message = $"Unknown namespace: {ToString()}",
                Severity = Feedback.SeverityType.Error,
                DebugInformation = DebugInformation
            });
            return DebugInformation.MakeErrorSymbol();
        }

        if (parentNs.TryGetSymbol(Identifier, out var sym)) return sym;

        ctx.Analysis.Feedbacks.Add(new Feedback
        {
            Message = $"Unknown symbol: {ToString()}",
            Severity = Feedback.SeverityType.Error,
            DebugInformation = DebugInformation
        });
        return DebugInformation.MakeErrorSymbol();
    }

    public Namespace GenerateNs(CodeGenContext ctx, Namespace rootNs = default)
    {
        var currentNs = ParentNs?.GenerateNs(ctx, rootNs) ?? rootNs ?? ctx.GlobalNamespace;
        return currentNs.TryGetNamespace(Identifier, out var res) ? res : currentNs.AddNamespace(Identifier);
    }

    public override string ToString()
    {
        return ParentNs == null ? Identifier : $"{ParentNs}::{Identifier}";
    }
}
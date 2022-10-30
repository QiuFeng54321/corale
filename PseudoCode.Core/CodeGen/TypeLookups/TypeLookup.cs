using PseudoCode.Core.Analyzing;
using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.Runtime.Errors;

namespace PseudoCode.Core.CodeGen.TypeLookups;

public class TypeLookup : AstNode
{
    private readonly string _name;
    private readonly TypeLookup _parent;

    public TypeLookup(string name, TypeLookup parent = default)
    {
        _name = name;
        _parent = parent;
    }

    /// <summary>
    ///     Tries to lookup a type starting from the <see cref="surroundingNs" /><br />
    ///     If the surrounding namespace doesn't contain the required type, it will search from its parent.
    /// </summary>
    /// <param name="ctx"></param>
    /// <param name="surroundingNs">The namespace to start searching from</param>
    /// <returns>The symbol or namespace found</returns>
    /// <exception cref="InvalidAccessError">
    ///     Thrown when required name is not found in any of the surrounding namespace and its
    ///     ancestors.
    /// </exception>
    public SymbolOrNamespace Lookup(CodeGenContext ctx, Namespace surroundingNs)
    {
        while (true)
        {
            // 找到
            var typeNs = _parent?.Lookup(ctx, surroundingNs).Ns ?? surroundingNs;
            if (typeNs.TryGetSymbol(_name, out var sym)) return new SymbolOrNamespace(sym);
            if (typeNs.TryGetNamespace(_name, out var nsFound)) return new SymbolOrNamespace(Ns: nsFound);

            surroundingNs = surroundingNs.Parent;
            if (surroundingNs == null)
            {
                ctx.Analysis.Feedbacks.Add(new Feedback
                {
                    Message = $"Unknown namespace or symbol: {_name}",
                    Severity = Feedback.SeverityType.Error,
                    DebugInformation = DebugInformation
                });
                return new SymbolOrNamespace(Symbol.ErrorSymbol);
            }
        }
    }

    public override string ToString()
    {
        return _parent == null ? _name : $"{_parent}.{_name}";
    }
}
using PseudoCode.Core.CodeGen.Containers;

namespace PseudoCode.Core.CodeGen.TypeLookups;

public class ModularType : AstNode
{
    private readonly GenericUtilisation _genericParameters;
    private readonly TypeLookup _typeLookup;

    public ModularType(TypeLookup typeLookup, GenericUtilisation genericParameters = default)
    {
        _typeLookup = typeLookup;
        _genericParameters = genericParameters;
    }

    public Symbol Lookup(CodeGenContext ctx, CompilationUnit cu, Function function, Namespace ns)
    {
        var symbol = _typeLookup.Lookup(ctx, ns).Symbol;
        if (_genericParameters != null)
            symbol = symbol.FillGeneric(ctx, cu, function,
                _genericParameters.GetSymbols(ctx, cu, function, ns));

        return symbol;
    }

    public override string ToString()
    {
        return _typeLookup.ToString() + _genericParameters;
    }
}
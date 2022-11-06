using PseudoCode.Core.CodeGen.Containers;

namespace PseudoCode.Core.CodeGen.TypeLookups;

public class DataType : AstNode
{
    // For pointer and array type
    private readonly DataType _elementType;
    private readonly ModularType _modularType;
    private readonly Expression CArrayLength;

    public DataType(ModularType modularType)
    {
        _modularType = modularType;
    }

    public DataType(DataType elementType)
    {
        _elementType = elementType;
    }

    /// <summary>
    ///     CArray
    /// </summary>
    /// <param name="elementType"></param>
    /// <param name="cArrayLength"></param>
    public DataType(DataType elementType, Expression cArrayLength)
    {
        _elementType = elementType;
        CArrayLength = cArrayLength;
    }

    public Symbol Lookup(CodeGenContext ctx, CompilationUnit cu, Function function, Namespace ns = default)
    {
        if (_modularType != null)
            return _modularType.Lookup(ctx, cu, function, ns ?? function.BodyNamespace);
        if (_elementType != null)
        {
            var elementSym = _elementType.Lookup(ctx, cu, function, ns);
            var ptrSym = elementSym.MakePointerType();
            if (CArrayLength == null) return ptrSym;
            var arrayLength = CArrayLength.CodeGen(ctx, cu, function);
            var arrayAlloca = cu.Builder.BuildArrayAlloca(elementSym.Type, arrayLength.GetRealValueRef(ctx, cu));
            var arrSym = Symbol.MakeTemp(ptrSym.Type, arrayAlloca, true);
            return arrSym;
        }

        throw new NotImplementedException();
    }

    public override string ToString()
    {
        if (_modularType != null) return _modularType.ToString();
        return "^" + _elementType;
    }
}
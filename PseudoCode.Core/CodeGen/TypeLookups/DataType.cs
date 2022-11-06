using PseudoCode.Core.CodeGen.Containers;

namespace PseudoCode.Core.CodeGen.TypeLookups;

public class DataType : AstNode
{
    private readonly Expression[] _cArrayLengths;

    // For pointer and array type
    private readonly DataType _elementType;
    private readonly ModularType _modularType;

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
    /// <param name="cArrayLengths"></param>
    public DataType(DataType elementType, Expression[] cArrayLengths)
    {
        _elementType = elementType;
        _cArrayLengths = cArrayLengths;
    }

    public Symbol Lookup(CodeGenContext ctx, CompilationUnit cu, Function function, Namespace ns = default)
    {
        if (_modularType != null)
            return _modularType.Lookup(ctx, cu, function, ns ?? function.BodyNamespace);
        if (_elementType != null)
        {
            var arrType = _elementType.Lookup(ctx, cu, function, ns).Type;
            var ptrSym = arrType.MakePointerType();
            if (_cArrayLengths == null || _cArrayLengths.Length == 0) return Symbol.MakeTypeSymbol(arrType, ns);
            Symbol arrTypeSym = null;
            foreach (var cArrayLength in _cArrayLengths.Reverse())
            {
                var arrayLength = cArrayLength.CodeGen(ctx, cu, function);
                var arrayLengthValueRef = arrayLength.GetRealValueRef(ctx, cu);
                if (arrayLengthValueRef.IsAConstantInt != null)
                {
                    var lengthInt = (long)arrayLength.ConstValue;
                    arrType = arrType.MakeArrayType((uint)lengthInt);
                }
                else
                {
                    arrType = arrType.MakePointerType();
                }
            }

            arrTypeSym = Symbol.MakeTypeSymbol(arrType, ns);
            return arrTypeSym;
        }

        throw new NotImplementedException();
    }

    public override string ToString()
    {
        if (_modularType != null) return _modularType.ToString();
        if (_cArrayLengths != null) return $"{_elementType}[{_cArrayLengths}]";
        return "^" + _elementType;
    }
}
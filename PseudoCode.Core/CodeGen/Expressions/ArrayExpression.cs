using LLVMSharp.Interop;
using PseudoCode.Core.CodeGen.Containers;

namespace PseudoCode.Core.CodeGen.Expressions;

public class ArrayExpression : Expression
{
    private readonly List<Expression> _elementExpressions;

    public ArrayExpression(List<Expression> elementExpressions)
    {
        _elementExpressions = elementExpressions;
    }

    public override Symbol CodeGen(CodeGenContext ctx, CompilationUnit cu, Function function)
    {
        // TODO: For non-constants, store manually
        var elementSymbols = _elementExpressions.Select(e => e.CodeGen(ctx, cu, function)).ToList();
        var arrType = elementSymbols[0].Type.MakeArrayType((uint)_elementExpressions.Count);
        var arrSto =
            LLVMValueRef.CreateConstArray(elementSymbols[0].Type,
                elementSymbols.Select(e => e.GetRealValueRef(ctx, cu)).ToArray());
        return Symbol.MakeTemp(arrType, arrSto);
    }

    public override string ToFormatString()
    {
        return $"[{string.Join(", ", _elementExpressions)}]";
    }
}
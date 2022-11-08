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
        var elementTy = elementSymbols[0].Type;
        var arrType = elementTy.MakeArrayType((uint)_elementExpressions.Count);
        var elementValRefs = elementSymbols.Select(e => e.GetRealValueRef(ctx, cu)).ToArray();
        LLVMValueRef arrSto;
        var isMem = false;
        if (elementValRefs.Any(e => !e.IsConstant))
        {
            arrSto = cu.Builder.BuildAlloca(arrType);
            for (var index = 0; index < elementValRefs.Length; index++)
            {
                var elementValRef = elementValRefs[index];
                var gep = cu.Builder.BuildInBoundsGEP2(elementTy, arrSto,
                    new[] { index.Const() });
                cu.Builder.BuildStore(elementValRef, gep);
            }

            isMem = true;
        }
        else
        {
            arrSto =
                LLVMValueRef.CreateConstArray(elementTy,
                    elementValRefs);
        }

        return Symbol.MakeTemp(arrType, arrSto, isMem);
    }

    public override string ToFormatString()
    {
        return $"[{string.Join(", ", _elementExpressions)}]";
    }
}
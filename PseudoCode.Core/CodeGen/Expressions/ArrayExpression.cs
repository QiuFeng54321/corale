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
        GetElementSymbols(ctx, cu, function, out var elementSymbols, out var elementTy, out var arrType);
        var elementValRefs = elementSymbols.Select(e => e.GetRealValueRef(ctx, cu)).ToArray();
        LLVMValueRef arrSto;
        var isMem = elementValRefs.Any(e => !e.IsConstant);
        if (isMem)
        {
            arrSto = cu.Builder.BuildAlloca(arrType);
            for (var index = 0; index < elementValRefs.Length; index++)
            {
                var elementValRef = elementValRefs[index];
                var gep = cu.Builder.BuildInBoundsGEP2(elementTy, arrSto,
                    new[] { index.Const() });
                cu.Builder.BuildStore(elementValRef, gep);
            }
        }
        else
        {
            arrSto = LLVMValueRef.CreateConstArray(elementTy, elementValRefs);
        }

        return Symbol.MakeTemp(arrType, arrSto, isMem);
    }

    public void GetElementSymbols(CodeGenContext ctx, CompilationUnit cu, Function function,
        out List<Symbol> elementSymbols,
        out Type elementTy, out Type arrType)
    {
        elementSymbols = _elementExpressions.Select(e => e.CodeGen(ctx, cu, function)).ToList();
        elementTy = elementSymbols[0].Type;
        arrType = elementTy.MakeArrayType((uint)_elementExpressions.Count);
    }

    public override string ToFormatString()
    {
        return $"[{string.Join(", ", _elementExpressions)}]";
    }
}
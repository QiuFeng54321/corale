using PseudoCode.Core.CodeGen.Containers;

namespace PseudoCode.Core.CodeGen.Operator;

public class ArrayPtrCasterResolver : NativeCasterResolver
{
    public override Symbol Resolve(Symbol from, Symbol toType, CodeGenContext ctx, CompilationUnit cu,
        Function function)
    {
        if (from.Type.Kind is not Types.CArray || toType.Type.Kind is not Types.Pointer)
            return Symbol.MakeErrorSymbol(from.DebugInformation);
        var arrAlloca = cu.Builder.BuildAlloca(from.Type);
        var arrSym = Symbol.MakeTemp(from.Type, arrAlloca, true);
        cu.Builder.BuildStore(from.GetRealValueRef(ctx, cu), arrAlloca);
        var casted = cu.Builder.BuildBitCast(arrSym.GetRealValueRef(ctx, cu), toType.Type);
        return Symbol.MakeTemp(toType.Type, casted);
    }
}
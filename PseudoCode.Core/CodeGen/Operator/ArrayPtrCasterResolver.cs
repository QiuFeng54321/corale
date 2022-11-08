using PseudoCode.Core.CodeGen.Containers;

namespace PseudoCode.Core.CodeGen.Operator;

public class ArrayPtrCasterResolver : NativeCasterResolver
{
    public override Symbol Resolve(Symbol from, Symbol toType, CodeGenContext ctx, CompilationUnit cu,
        Function function)
    {
        if (from.Type.Kind is not Types.CArray || toType.Type.Kind is not Types.Pointer)
            return Symbol.MakeErrorSymbol(from.DebugInformation);
        // var list = new List<LLVMValueRef>() {PseudoInteger.Zero};
        // var curType = from.Type?.ElementType;
        // while (curType is {})
        // {
        //     list.Add(PseudoInteger.Zero);
        //     curType = curType.ElementType;
        // }
        // var arrAlloca = cu.Builder.BuildInBoundsGEP2(from.Type, from.GetRealValueRef(ctx, cu),
        //     list.ToArray(), "gep");
        var tmpAlloca = cu.Builder.BuildAlloca(from.Type);
        cu.Builder.BuildStore(from.GetRealValueRef(ctx, cu), tmpAlloca);
        var ptr = cu.Builder.BuildBitCast(tmpAlloca, toType.Type);
        return Symbol.MakeTemp(toType.Type, ptr);
    }
}
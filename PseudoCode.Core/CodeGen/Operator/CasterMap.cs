using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.CodeGen.Expressions;

namespace PseudoCode.Core.CodeGen.Operator;

public class CasterMap
{
    public readonly Symbol Casters = new(ReservedNames.Caster, false, null);
    public readonly List<NativeCasterResolver> NativeCasterResolvers = new();

    public Symbol Cast(Symbol from, Symbol toType, CodeGenContext ctx, CompilationUnit cu, Function function)
    {
        foreach (var nativeCasterResolver in NativeCasterResolvers)
        {
            var res = nativeCasterResolver.Resolve(from, toType, ctx, cu, function);
            if (!res.IsError) return res;
        }

        var args = new[] { from };
        var overload = Casters.FindFunctionOverload(args.ToList(), toType);
        return CallExpression.CodeGenCallFunc(ctx, cu, overload, args);
    }

    public bool TryAddCaster(Symbol overload)
    {
        return Casters.AddOverload(overload);
    }

    public bool TryAddNativeCaster(NativeCasterResolver nativeCasterResolver)
    {
        if (NativeCasterResolvers.Contains(nativeCasterResolver)) return false;
        NativeCasterResolvers.Add(nativeCasterResolver);
        return true;
    }
}
using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.Runtime.Errors;
using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.CodeGen.Operator;

public class OperatorResolverMap
{
    private readonly Dictionary<Types, OperatorResolver> _operatorResolvers = new();
    public TypeOperatorResolver TypeOperatorResolver;

    public void AddResolver(Types type, OperatorResolver resolver)
    {
        resolver.ResolverMap = this;
        _operatorResolvers.Add(type, resolver);
    }

    public void Initialize()
    {
        AddResolver(Types.Integer, new IntegerOperatorResolver());
        AddResolver(Types.Character, new CharacterOperatorResolver());
        AddResolver(Types.Real, new RealOperatorResolver());
        AddResolver(Types.Boolean, new BooleanOperatorResolver());
        AddResolver(Types.Pointer, new PointerOperatorResolver());
        AddResolver(Types.Type, TypeOperatorResolver = new TypeOperatorResolver());
    }

    public Symbol Resolve(Symbol left, Symbol right, PseudoOperator op, CodeGenContext ctx, CompilationUnit cu)
    {
        if (op is PseudoOperator.None) throw new InvalidAccessError(op.ToString());
        if (left.Type.Kind is Types.None) throw new InvalidTypeError("Left is none");
        var res = _operatorResolvers[left.Type.Kind].Resolve(left, right, op, ctx, cu);
        if (res == null)
        {
            res = _operatorResolvers[Types.Type].Resolve(left, right, op, ctx, cu); // Custom operator possibly
            if (res == null) return null;
        }

        return res;
    }
}
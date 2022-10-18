using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.Runtime.Errors;
using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.CodeGen.Operator;

public class OperatorResolverMap
{
    private readonly Dictionary<Types, OperatorResolver> _operatorResolvers = new();

    public void AddResolver(Types type, OperatorResolver resolver)
    {
        resolver.ResolverMap = this;
        _operatorResolvers.Add(type, resolver);
    }

    public void Initialize()
    {
        AddResolver(Types.Integer, new IntegerOperatorResolver());
        AddResolver(Types.Real, new RealOperatorResolver());
        AddResolver(Types.Boolean, new BooleanOperatorResolver());
    }

    public Symbol Resolve(Symbol left, Symbol right, PseudoOperator op, CodeGenContext ctx)
    {
        if (op is PseudoOperator.None) throw new InvalidAccessError(op.ToString());
        if (left.Type.Kind is Types.None) throw new InvalidTypeError("Left is none");
        var res = _operatorResolvers[left.Type.Kind].Resolve(left, right, op, ctx);
        if (res == null)
        {
            res = _operatorResolvers[Types.Type].Resolve(left, right, op, ctx); // Custom operator possibly
            if (res == null) throw new InvalidTypeError($"{left.Type} {op} {right?.Type}");
        }

        return res;
    }
}
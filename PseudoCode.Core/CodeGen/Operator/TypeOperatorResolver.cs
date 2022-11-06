using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.CodeGen.Expressions;
using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.CodeGen.Operator;

public class TypeOperatorResolver : OperatorResolver
{
    /// <summary>
    ///     Stores a map of Function Group for each of the <see cref="PseudoOperator" />
    /// </summary>
    public readonly Dictionary<PseudoOperator, Symbol> Operators = new();

    public override Symbol Resolve(Symbol left, Symbol right, PseudoOperator op, CodeGenContext ctx, CompilationUnit cu)
    {
        if (!Operators.TryGetValue(op, out var operatorFuncGroup)) return null;

        var args = right == null ? new[] { left } : new[] { left, right };
        var overload = operatorFuncGroup.FindFunctionOverload(args.ToList());
        return CallExpression.CodeGenCallFunc(ctx, cu, overload, args);
    }

    public bool TryAddOperator(PseudoOperator @operator, Symbol overload, out Symbol funcGroup)
    {
        if (!Operators.ContainsKey(@operator))
            Operators.Add(@operator, Symbol.MakeFunctionGroupSymbol(@operator.ToString(), null));
        funcGroup = Operators[@operator];
        return funcGroup.AddOverload(overload);
    }
}
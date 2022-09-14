using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.CodeGen;

public static class Helper
{
    public static bool IsComparison(this PseudoOperator op)
    {
        return op is PseudoOperator.Equal or PseudoOperator.Greater or PseudoOperator.GreaterEqual
            or PseudoOperator.Smaller or PseudoOperator.SmallerEqual;
    }

    public static bool IsUnary(this PseudoOperator op)
    {
        return op is PseudoOperator.Not or PseudoOperator.Negative;
    }
}
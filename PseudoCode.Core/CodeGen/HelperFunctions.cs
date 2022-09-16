using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.CodeGen;

public static class HelperFunctions
{
    public static readonly Dictionary<PseudoOperator, string> OperatorFormattedStrings = new()
    {
        [PseudoOperator.Add] = "+",
        [PseudoOperator.Subtract] = "-",
        [PseudoOperator.Multiply] = "*",
        [PseudoOperator.Divide] = "/",
        [PseudoOperator.IntDivide] = "DIV",
        [PseudoOperator.Pow] = "POW",
        [PseudoOperator.Negative] = "-",
        [PseudoOperator.Not] = "NOT ",
        [PseudoOperator.And] = "AND",
        [PseudoOperator.Or] = "OR",
        [PseudoOperator.GetPointer] = "^",
        [PseudoOperator.GetPointed] = "^",
        [PseudoOperator.BitAnd] = "&",
        [PseudoOperator.Greater] = ">",
        [PseudoOperator.GreaterEqual] = ">=",
        [PseudoOperator.Smaller] = "<",
        [PseudoOperator.SmallerEqual] = "<=",
        [PseudoOperator.Equal] = "="
    };

    public static string ToFormattedString(this PseudoOperator op)
    {
        return OperatorFormattedStrings[op];
    }

    public static Dictionary<string, Type> Clone(this Dictionary<string, Type> source)
    {
        return source.ToDictionary(p => p.Key, p => p.Value.Clone());
    }

    public static Dictionary<string, Symbol> Clone(this Dictionary<string, Symbol> source)
    {
        return source.ToDictionary(p => p.Key, p => p.Value.Clone());
    }

    public static List<Symbol> Clone(this IEnumerable<Symbol> source)
    {
        return source.Select(s => s.Clone()).ToList();
    }

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
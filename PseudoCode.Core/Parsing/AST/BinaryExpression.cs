using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.Parsing.AST;

public class BinaryExpression : Expression
{
    public PseudoOperator Operator;
    public Expression Left, Right;
}
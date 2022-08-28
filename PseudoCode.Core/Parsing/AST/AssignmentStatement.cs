namespace PseudoCode.Core.Parsing.AST;

public class AssignmentStatement : Statement
{
    public Expression Target, Value;
}
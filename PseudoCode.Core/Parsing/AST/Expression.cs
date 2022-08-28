namespace PseudoCode.Core.Parsing.AST;

public abstract class Expression : AstNode
{
    public abstract bool CanEvaluate();
    public abstract object Evaluate();
    public 
}
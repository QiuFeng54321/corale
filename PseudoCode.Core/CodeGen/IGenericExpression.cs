namespace PseudoCode.Core.CodeGen;

public interface IGenericExpression
{
    Symbol Generate(CodeGenContext ctx, Block block, List<Symbol> genericParams = default);
}
using PseudoCode.Core.CodeGen.Containers;

namespace PseudoCode.Core.CodeGen.Expressions;

public interface IGenericExpression
{
    Symbol Generate(CodeGenContext ctx, CompilationUnit cu, Function function, List<Symbol> genericParams = default);
}
using PseudoCode.Core.CodeGen.Containers;

namespace PseudoCode.Core.CodeGen;

public interface IGenericExpression
{
    Symbol Generate(CodeGenContext ctx, CompilationUnit cu, Function function, List<Symbol> genericParams = default);
}
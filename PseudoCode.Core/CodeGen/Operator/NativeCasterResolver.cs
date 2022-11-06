using PseudoCode.Core.CodeGen.Containers;

namespace PseudoCode.Core.CodeGen.Operator;

public abstract class NativeCasterResolver
{
    public abstract Symbol Resolve(Symbol from, Symbol toType, CodeGenContext ctx, CompilationUnit cu,
        Function function);
}
using PseudoCode.Core.CodeGen.Containers;

namespace PseudoCode.Core.CodeGen;

public abstract class Expression : AstNode
{
    public abstract Symbol CodeGen(CodeGenContext ctx, CompilationUnit cu, Function function);

    public abstract string ToFormatString();

    public override string ToString()
    {
        return ToFormatString();
    }
}
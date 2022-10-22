using PseudoCode.Core.CodeGen.Containers;

namespace PseudoCode.Core.CodeGen;

public abstract class Expression : AstNode
{
    public abstract Symbol CodeGen(CodeGenContext ctx, Function function);

    public abstract string ToFormatString();
}
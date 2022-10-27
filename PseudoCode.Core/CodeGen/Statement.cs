using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.Formatting;

namespace PseudoCode.Core.CodeGen;

public abstract class Statement : AstNode, IPseudoFormattable
{
    public abstract void Format(PseudoFormatter formatter);
    public abstract void CodeGen(CodeGenContext ctx, CompilationUnit cu, Function function);

    public override string ToString()
    {
        using var strWriter = new StringWriter();
        using var formatter = new PseudoFormatter(strWriter);
        Format(new PseudoFormatter(strWriter));
        return strWriter.ToString();
    }
}
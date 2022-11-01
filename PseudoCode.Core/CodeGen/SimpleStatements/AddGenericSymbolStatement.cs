using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.Formatting;

namespace PseudoCode.Core.CodeGen.SimpleStatements;

public class AddGenericSymbolStatement : Statement
{
    public Symbol Symbol;

    public override void Format(PseudoFormatter formatter)
    {
    }

    public override void CodeGen(CodeGenContext ctx, CompilationUnit cu, Function function)
    {
        function.BodyNamespace.AddSymbol(Symbol);
    }
}
using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.Formatting;

namespace PseudoCode.Core.CodeGen;

public class ImportStatement : Statement
{
    public readonly string Path;

    public ImportStatement(string path)
    {
        Path = path;
    }

    public override void Format(PseudoFormatter formatter)
    {
        formatter.WriteStatement($"IMPORT {Path}");
    }

    public override void CodeGen(CodeGenContext ctx, CompilationUnit cu, Function function)
    {
        cu.ImportUnit(ctx, Path);
        cu.Builder.PositionAtEnd(function.CurrentBlockRef);
    }
}
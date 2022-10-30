using PseudoCode.Core.CodeGen.Containers;
using PseudoCode.Core.Runtime;

namespace PseudoCode.Core.CodeGen;

public class DebugInformation
{
    public DebugInformation(CompilationUnit compilationUnit, SourceRange fullSourceRange,
        SourceRange specificSourceRange = default)
    {
        CompilationUnit = compilationUnit;
        FullSourceRange = fullSourceRange;
        SpecificSourceRange = specificSourceRange ?? fullSourceRange;
    }

    public CompilationUnit CompilationUnit { get; }
    public SourceRange FullSourceRange { get; }
    public SourceRange SpecificSourceRange { get; }

    public override string ToString()
    {
        var fileName = Path.GetFileName(CompilationUnit.FilePath);
        var srStr = SpecificSourceRange.ToString();
        if (FullSourceRange != SpecificSourceRange) srStr += $", broadly {FullSourceRange}";

        return $"{fileName} {srStr}";
    }

    public void Deconstruct(out CompilationUnit compilationUnit, out SourceRange fullSourceRange,
        out SourceRange specificSourceRange)
    {
        compilationUnit = CompilationUnit;
        fullSourceRange = FullSourceRange;
        specificSourceRange = SpecificSourceRange;
    }
}
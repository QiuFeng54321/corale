using PseudoCode.Core.Runtime;

namespace PseudoCode.Core.CodeGen;

public record DebugInformation(string FileName, SourceRange FullSourceRange, SourceRange SpecificSourceRange);
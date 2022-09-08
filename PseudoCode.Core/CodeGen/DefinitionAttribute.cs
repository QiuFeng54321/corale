namespace PseudoCode.Core.CodeGen;

[Flags]
public enum DefinitionAttribute
{
    None,
    Const = 1 << 0, // Immediate value
    Immutable = 1 << 1, // Constant
    Reference = 1 << 2 // Variable
}
namespace PseudoCode.Core.Runtime;

[Flags]
public enum DefinitionAttribute
{
    None,
    Type = 1, // Type definition
    Const = 1 << 1, // Immediate value
    Immutable = 1 << 2, // Constant
    Reference = 1 << 3 // Variable
}
namespace PseudoCode.Core.CodeGen;

[Flags]
public enum DefinitionAttribute
{
    None,
    Const = 1 << 0, // Immediate value
    Immutable = 1 << 1, // Constant

    /// <summary>
    ///     BYREF: convert to ptr, assignment to parameter will change its value outside
    /// </summary>
    Reference = 1 << 2 // Variable
}
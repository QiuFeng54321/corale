namespace PseudoCode.Core.Runtime.Emit;

public record Identifier(string Name, PseudoScope Scope, MemoryAddress Address = default, TypeDefinition Definition = default)
{
}
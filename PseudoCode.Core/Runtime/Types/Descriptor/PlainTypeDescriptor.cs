using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime.Types.Descriptor;

public record PlainTypeDescriptor(string Name) : ITypeDescriptor
{
    public Definition GetDefinition(Scope scope, PseudoProgram program)
    {
        return scope.FindDefinition(SelfName);
    }

    public string SelfName => Name;

    public override string ToString() => SelfName;
}
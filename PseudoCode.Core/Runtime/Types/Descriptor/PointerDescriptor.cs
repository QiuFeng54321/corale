using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime.Types.Descriptor;

public record PointerDescriptor(ITypeDescriptor TypeDescriptor) : ITypeDescriptor
{
    public Definition GetDefinition(Scope scope, PseudoProgram program)
    {
        throw new NotImplementedException();
    }

    public string SelfName => "POINTER";
}
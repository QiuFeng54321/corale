using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime.Types.Descriptor;

public interface ITypeDescriptor
{
    public Definition GetDefinition(Scope scope, PseudoProgram program);

    public Type GetType(Scope scope, PseudoProgram program)
    {
        return GetDefinition(scope, program)?.Type ?? new NullType(scope, program);
    }
    public string SelfName { get; }
}
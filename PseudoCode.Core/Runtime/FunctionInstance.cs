using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime;

public class FunctionInstance : Instance
{
    public FunctionType FunctionType => (FunctionType)Type;
    public Scope FunctionBody;
    public FunctionInstance(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }
}
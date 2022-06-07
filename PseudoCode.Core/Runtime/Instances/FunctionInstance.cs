using PseudoCode.Core.Runtime.Operations;
using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.Runtime.Instances;

public class FunctionInstance : Instance
{
    public Scope FunctionBody;

    public FunctionInstance()
    {
    }

    public FunctionInstance(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public FunctionType FunctionType => (FunctionType)Type;
}
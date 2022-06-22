using PseudoCode.Core.Runtime.Operations;
using PseudoCode.Core.Runtime.Reflection;
using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.Runtime.Instances;


public class BuiltinFunctionInstance : FunctionInstance
{
    public BuiltinFunctionInstance()
    {
    }

    public BuiltinFunctionInstance(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public FunctionBinder.BuiltinFunction Func
    {
        get => (FunctionBinder.BuiltinFunction)Value;
        set => Value = value;
    }

    public BuiltinFunctionType BuiltinFunctionType => (BuiltinFunctionType)Type;
}
using PseudoCode.Core.Runtime.Operations;
using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.Runtime.Instances;

using FuncType = Func<Scope, PseudoProgram, Instance[], Instance>;

public class BuiltinFunctionInstance : FunctionInstance
{
    public BuiltinFunctionInstance(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public FuncType Func
    {
        get => (FuncType)Value;
        set => Value = value;
    }

    public BuiltinFunctionType BuiltinFunctionType => (BuiltinFunctionType)Type;
}
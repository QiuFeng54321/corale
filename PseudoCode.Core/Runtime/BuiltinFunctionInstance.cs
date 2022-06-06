using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime;
using FuncType = Func<Scope, PseudoProgram, Instance[], Instance>;
public class BuiltinFunctionInstance : FunctionInstance
{
    public FuncType Func
    {
        get => (FuncType)Value;
        set => Value = value;
    }
    
    public BuiltinFunctionType BuiltinFunctionType => (BuiltinFunctionType)Type;
    public BuiltinFunctionInstance(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }
}
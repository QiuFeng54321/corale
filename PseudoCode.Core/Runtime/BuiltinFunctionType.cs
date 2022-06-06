using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime;

public class BuiltinFunctionType : FunctionType
{
    public BuiltinFunctionType(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }
    public override Instance Instance(object value = null, Scope scope = null)
    {
        var instance = new BuiltinFunctionInstance(scope ?? ParentScope, Program)
        {
            Type = this,
            Members = new Dictionary<string, Instance>(),
            Value = value
        };
        foreach (var member in Members) instance.Members[member.Key] = member.Value.Instance(scope: ParentScope);

        return instance;
    }
    public override Instance Call(FunctionInstance functionInstance, Instance[] args)
    {
        CheckArguments(args);
        return ((BuiltinFunctionInstance)functionInstance).Func(ParentScope, Program, args);
    }
}
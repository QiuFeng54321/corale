using PseudoCode.Core.Runtime.Errors;
using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Reflection;

namespace PseudoCode.Core.Runtime.Operations;

public class MakeBuiltinFunctionOperation : Operation
{
    public Definition Definition;
    public FunctionBinder.BuiltinFunction Func;
    public string Name;

    public MakeBuiltinFunctionOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        var instance = ParentScope.FindDefinition(Name).Type.Instance(Func);
        if (instance is not BuiltinFunctionInstance functionInstance)
            throw new InvalidTypeError($"I'm making a function of {instance.Type}???", this);
        ParentScope.ScopeStates.InstanceAddresses.Add(Name, Program.AllocateId(functionInstance));
    }

    public override void MetaOperate()
    {
        base.MetaOperate();
        ParentScope.AddVariableDefinition(Name, Definition, Definition.SourceRange);
    }

    public override string ToPlainString()
    {
        return $"Make built-in function {Name}: {Definition.Type}";
    }

    public override string ToString(int depth)
    {
        return $"{Indent(depth)}{ToPlainString()}";
    }
}
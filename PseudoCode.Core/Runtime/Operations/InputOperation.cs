namespace PseudoCode.Core.Runtime.Operations;

public class InputOperation : Operation
{
    public InputOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        var instance = Program.RuntimeStack.Pop();
        var input = Console.ReadLine();
        instance.Type.Assign(instance, ParentScope.FindType(Type.StringId).Instance(input, ParentScope));
    }

    public override void MetaOperate()
    {
        base.MetaOperate();
        var type = Program.TypeCheckStack.Pop();
        if (type is PlaceholderType placeholderType) placeholderType.MetaAssign(ParentScope.FindType(Type.StringId));
    }

    public override string ToPlainString()
    {
        return strings.InputOperation_ToPlainString;
    }
}
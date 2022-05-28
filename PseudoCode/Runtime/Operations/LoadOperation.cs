using PseudoCode.Runtime.Errors;

namespace PseudoCode.Runtime.Operations;

public class LoadOperation : Operation
{
    public string LoadName;

    public LoadOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        try
        {
            ParentScope.RuntimeStack.Push(new ReferenceInstance(ParentScope, Program)
                { ReferenceAddress = ParentScope.FindInstanceAddress(LoadName) });
        }
        catch (InvalidAccessError)
        {
            Console.WriteLine($"Warning: {LoadName} is not found in current scope. Creating one...");
            ParentScope.RuntimeStack.Push(ParentScope.FindType(Type.PlaceholderId).Instance(LoadName));
        }
    }

    public override string ToPlainString()
    {
        return $"Push ref {LoadName}";
    }
}
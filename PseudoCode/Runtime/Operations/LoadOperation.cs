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
            Program.RuntimeStack.Push(new ReferenceInstance(ParentScope, Program)
                { ReferenceAddress = ParentScope.FindInstanceAddress(LoadName) });
        }
        catch (InvalidAccessError)
        {
            Console.WriteLine($"Warning: {LoadName} is not found in current scope. Creating one...");
            Program.RuntimeStack.Push(ParentScope.FindType(Type.PlaceholderId).Instance(LoadName, ParentScope));
        }
    }

    public override string ToPlainString()
    {
        return $"Push ref {LoadName}";
    }
}
using PseudoCode.Core.Runtime.Errors;

namespace PseudoCode.Core.Runtime.Operations;

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
            if (!Program.AllowUndeclaredVariables) throw;
            // Console.WriteLine($"Warning: {LoadName} is not found in current scope. Creating one...");
            Program.RuntimeStack.Push(ParentScope.FindType(Type.PlaceholderId).Instance(LoadName, ParentScope));
        }
    }

    public override string ToPlainString()
    {
        return string.Format(strings.LoadOperation_ToPlainString, LoadName);
    }
}
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
            Program.RuntimeStack.Push(ParentScope.FindTypeDefinition(Type.PlaceholderId).Type.Instance(LoadName, ParentScope));
        }
    }

    public override void MetaOperate()
    {
        base.MetaOperate();
        var type = ParentScope.FindInstanceDefinition(LoadName);
        if (type == null)
        {
            ParentScope.InstanceDefinitions.Add(LoadName, new Definition
            {
                Name = LoadName,
                Type = new PlaceholderType(ParentScope, Program)
                {
                    InstanceName = LoadName
                },
                SourceLocation = PoiLocation
            });
        }

        Program.TypeCheckStack.Push(ParentScope.FindInstanceDefinition(LoadName).Type);
    }

    public override string ToPlainString()
    {
        return string.Format(strings.LoadOperation_ToPlainString, LoadName);
    }
}
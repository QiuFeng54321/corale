using PseudoCode.Core.Runtime.Errors;
using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Types;
using Type = PseudoCode.Core.Runtime.Types.Type;

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
            Program.RuntimeStack.Push(Program.FindTypeDefinition(Type.PlaceholderId).Type
                .Instance(LoadName, ParentScope));
        }
    }

    public override void MetaOperate()
    {
        base.MetaOperate();
        var definition = ParentScope.FindInstanceDefinition(LoadName);
        definition?.References?.Add(SourceRange);
        if (definition == null)
            ParentScope.AddVariableDefinition(LoadName, new Definition (ParentScope, Program)
            {
                Name = LoadName,
                Type = new PlaceholderType(ParentScope, Program)
                {
                    InstanceName = LoadName
                },
                SourceRange = SourceRange,
                References = new List<SourceRange> { SourceRange }
            }, SourceRange);

        Program.TypeCheckStack.Push(new TypeInfo {
            Type = ParentScope.FindInstanceDefinition(LoadName).Type,
            IsReference = true,
            SourceRange = SourceRange
        });
    }

    public override string ToPlainString()
    {
        return string.Format(strings.LoadOperation_ToPlainString, LoadName);
    }
}
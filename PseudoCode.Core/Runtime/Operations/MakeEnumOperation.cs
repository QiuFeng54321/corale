using PseudoCode.Core.Analyzing;
using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.Runtime.Operations;

public class MakeEnumOperation : Operation
{
    public string Name;
    public List<string> Names;
    public readonly List<Definition> ElementDefinitions = new();
    public MakeEnumOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        foreach (var definition in ElementDefinitions)
        {
            ParentScope.ScopeStates.InstanceAddresses.Add(definition.Name, Program.AllocateId(definition.ConstantInstance));
        }
    }

    public override void MetaOperate()
    {
        base.MetaOperate();
        var enumType = new EnumType(ParentScope, Program)
        {
            Names = Names
        };
        ParentScope.AddTypeDefinition(Name, new Definition(ParentScope, Program)
        {
            Type = enumType,
            Name = Name,
            SourceRange = SourceRange,
            References = new List<SourceRange> { SourceRange },
            Attributes = Definition.Attribute.Type
        }, SourceRange);
        for (var index = 0; index < Names.Count; index++)
        {
            var elementName = Names[index];
            var definition = new Definition(ParentScope, Program)
            {
                Name = elementName,
                Type = enumType,
                SourceRange = SourceRange,
                Attributes = Definition.Attribute.Immutable | Definition.Attribute.Const,
                References = new List<SourceRange> { SourceRange },
                ConstantInstance = enumType.Instance(index, ParentScope)
            };
            ElementDefinitions.Add(definition);
            ParentScope.AddVariableDefinition(elementName, definition, SourceRange);
        }
    }

    public override string ToPlainString() => $"Enum {Name} = ({string.Join(", ", Names)})";
}
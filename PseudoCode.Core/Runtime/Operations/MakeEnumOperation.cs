using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.Runtime.Operations;

public class MakeEnumOperation : Operation
{
    public readonly List<Definition> ElementDefinitions = new();
    public string Name;
    public Dictionary<int, string> Names;
    public Dictionary<string, int> Values;

    public MakeEnumOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
        foreach (var definition in ElementDefinitions)
        {
            ParentScope.ScopeStates.InstanceAddresses.Add(definition.Name,
                Program.AllocateId(definition.ConstantInstance));
        }
    }

    public override void MetaOperate()
    {
        base.MetaOperate();
        var enumType = new EnumType(ParentScope, Program)
        {
            Values = Values,
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
        foreach (var (name, value) in Values)
        {
            var definition = new Definition(ParentScope, Program)
            {
                Name = name,
                Type = enumType,
                SourceRange = SourceRange,
                Attributes = Definition.Attribute.Immutable | Definition.Attribute.Const,
                References = new List<SourceRange> { SourceRange },
                ConstantInstance = enumType.Instance(value, ParentScope)
            };
            ElementDefinitions.Add(definition);
            ParentScope.AddVariableDefinition(name, definition, SourceRange);
        }
    }

    public override string ToPlainString()
    {
        return $"Enum {Name} = ({string.Join(", ", Values)})";
    }
}
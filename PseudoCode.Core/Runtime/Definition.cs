using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Operations;
using PseudoCode.Core.Runtime.Types;
using PseudoCode.Core.Runtime.Types.Descriptor;
using Type = PseudoCode.Core.Runtime.Types.Type;

namespace PseudoCode.Core.Runtime;

public record Definition(Scope ParentScope, PseudoProgram Program)
{
    private Type _type;
    public virtual string Name { get; set; }
    public virtual List<SourceRange> References { get; set; } = new();
    public virtual SourceRange SourceRange { get; set; }
    public virtual Scope ParentScope { get; set; } = ParentScope;
    public virtual PseudoProgram Program { get; set; } = Program;
    public virtual DefinitionAttribute Attributes { get; set; } = DefinitionAttribute.Reference;
    public virtual string TypeName => TypeDescriptor?.SelfName ?? Type.Name;

    public virtual Instance ConstantInstance { get; set; }

    public Type Type
    {
        get => _type ??= TypeDescriptor?.GetType(ParentScope, Program) ?? new NullType(ParentScope, Program);
        set => _type = value;
    }

    public virtual ITypeDescriptor TypeDescriptor { get; set; }

    public string TypeString()
    {
        return TypeDescriptor?.ToString() ?? Type.ToString();
    }

    public override string ToString()
    {
        return $"{GetAttributesString()}{TypeString()}";
    }

    public string GetAttributesString()
    {
        var res = (from DefinitionAttribute flagToCheck in Enum.GetValues(typeof(DefinitionAttribute))
            where Attributes.HasFlag(flagToCheck) && flagToCheck != DefinitionAttribute.None
            select flagToCheck.ToString().ToUpper()).ToList();

        return string.Join(" ", res) + (res.Count == 0 ? "" : " ");
    }

    public Definition Make(string name, DefinitionAttribute attributes)
    {
        return this with
        {
            Name = name,
            Attributes = attributes
        };
    }
}
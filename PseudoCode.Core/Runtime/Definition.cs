using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Operations;
using PseudoCode.Core.Runtime.Types;
using PseudoCode.Core.Runtime.Types.Descriptor;
using Type = PseudoCode.Core.Runtime.Types.Type;

namespace PseudoCode.Core.Runtime;

public record Definition(Scope ParentScope, PseudoProgram Program)
{
    [Flags]
    public enum Attribute
    {
        None,
        Type = 1, // Type definition
        Const = 1 << 1, // Immediate value
        Immutable = 1 << 2, // Constant
        Reference = 1 << 3 // Variable
    }

    private Type _type;
    public virtual string Name { get; set; }
    public virtual List<SourceRange> References { get; set; } = new();
    public virtual SourceRange SourceRange { get; set; }
    public virtual Scope ParentScope { get; set; } = ParentScope;
    public virtual PseudoProgram Program { get; set; } = Program;
    public virtual Attribute Attributes { get; set; } = Attribute.Reference;
    public virtual string TypeName => TypeDescriptor?.SelfName ?? Type.Name;
    public virtual string Documentation { get; set; }

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

    public string TypeMarkupString() => TypeDescriptor?.ToMarkupString() ?? Type.ToString();

    public override string ToString()
    {
        return $"{GetAttributesStringIndented()}{TypeString()}";
    }

    public string ToMarkupString()
    {
        var attrStr = GetAttributesString();
        attrStr = string.IsNullOrEmpty(attrStr) ? "" : $"*{attrStr}* ";
        return $"{attrStr}{TypeMarkupString()}";
    }

    public string GetAttributesString()
    {
        var res = (from Attribute flagToCheck in Enum.GetValues(typeof(Attribute))
            where Attributes.HasFlag(flagToCheck) && flagToCheck != Attribute.None
            select flagToCheck.ToString().ToUpper()).ToList();

        return string.Join(" ", res);
    }

    public string GetAttributesStringIndented()
    {
        var res = (from Attribute flagToCheck in Enum.GetValues(typeof(Attribute))
            where Attributes.HasFlag(flagToCheck) && flagToCheck != Attribute.None
            select flagToCheck.ToString().ToUpper()).ToList();

        return string.Join(" ", res) + (res.Count == 0 ? "" : " ");
    }

    public Definition Make(string name, Attribute attributes)
    {
        return this with
        {
            Name = name,
            Attributes = attributes
        };
    }
}
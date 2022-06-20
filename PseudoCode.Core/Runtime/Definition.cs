using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Operations;
using PseudoCode.Core.Runtime.Types;
using Type = PseudoCode.Core.Runtime.Types.Type;

namespace PseudoCode.Core.Runtime;

// TODO: Type representation is A MESS. Find a new way to represent them
// instead of using Definition, TypeDescriptor, ParameterInfo, TypeInfo...
public record Definition(Scope ParentScope, PseudoProgram Program)
{
    public virtual string Name { get; set; }
    public virtual List<SourceRange> References { get; set; } = new();
    public virtual SourceRange SourceRange { get; set; }
    public virtual Scope ParentScope { get; set; } = ParentScope;
    public virtual PseudoProgram Program { get; set; } = Program;
    public virtual Attribute Attributes { get; set; } = Attribute.Variable;
    public virtual string TypeName => TypeDescriptor?.Name ?? Type.Name;

    public override string ToString()
    {
        return TypeDescriptor?.ToString() ?? Type.ToString();
    }

    [Flags]
    public enum Attribute
    {
        None,
        Type,
        Const,
        Immutable,
        Variable
    }

    public Type Type
    {
        get => _type ??= TypeDescriptor.GetType(ParentScope, Program);
        set => _type = value;
    }

    private Type _type { get; set; }
    public virtual Type.TypeDescriptor TypeDescriptor { get; set; }
    public virtual Instance DefaultInstance { get; set; }
}
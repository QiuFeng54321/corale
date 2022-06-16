using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Operations;
using PseudoCode.Core.Runtime.Types;
using Type = PseudoCode.Core.Runtime.Types.Type;

namespace PseudoCode.Core.Runtime;

public record Definition
{
    public Definition(Scope parentScope, PseudoProgram program)
    {
        ParentScope = parentScope;
        Program = program;
    }

    public string Name;
    public List<SourceRange> References = new();
    public SourceRange SourceRange;
    public Scope ParentScope;
    public PseudoProgram Program;
    public Attribute Attributes = Attribute.Variable;
    public string TypeName => TypeDescriptor?.Name ?? Type.Name;
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

    private Type _type;
    public Type.TypeDescriptor TypeDescriptor;
    public Instance DefaultInstance;
}
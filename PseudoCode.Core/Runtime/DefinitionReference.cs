using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Operations;
using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.Runtime;

public record DefinitionReference(Scope ParentScope, PseudoProgram Program) : Definition(ParentScope, Program)
{
    private Definition _definition;
    public int TypeId = -1;

    public Definition RealDefinition => _definition ??=
        Name == null ? ParentScope.FindDefinition((uint)TypeId) : ParentScope.FindDefinition(Name);

    public override Attribute Attributes => RealDefinition.Attributes;
    public override List<SourceRange> References => RealDefinition.References;
    public override Instance ConstantInstance => RealDefinition.ConstantInstance;
    public override SourceRange SourceRange => RealDefinition.SourceRange;
    public override TypeDescriptor TypeDescriptor => RealDefinition.TypeDescriptor;
    public override string TypeName => RealDefinition.TypeName;
}
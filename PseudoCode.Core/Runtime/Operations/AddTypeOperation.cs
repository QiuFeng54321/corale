using PseudoCode.Core.Runtime.Types.Descriptor;

namespace PseudoCode.Core.Runtime.Operations;

public class AddTypeOperation : Operation
{
    public string Name;
    public ITypeDescriptor TypeDescriptor;
    public AddTypeOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void MetaOperate()
    {
        base.MetaOperate();
        ParentScope.AddTypeDefinition(Name, new Definition(ParentScope, Program)
        {
            TypeDescriptor = TypeDescriptor,
            Name = Name,
            SourceRange = SourceRange,
            References = new List<SourceRange> { SourceRange },
            Attributes = DefinitionAttribute.Type
        }, SourceRange);
    }

    public override string ToPlainString() => $"Type alias {Name} = {TypeDescriptor}";
}
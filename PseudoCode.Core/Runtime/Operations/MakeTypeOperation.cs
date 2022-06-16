using PseudoCode.Core.Runtime.Types;

namespace PseudoCode.Core.Runtime.Operations;

public class MakeTypeOperation : Operation
{
    public Scope TypeBody;
    public string Name;

    public MakeTypeOperation(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public override void Operate()
    {
        base.Operate();
    }

    public override void MetaOperate()
    {
        base.MetaOperate();
        TypeBody.MetaOperate();
        ParentScope.AddTypeDefinition(Name, new Definition (ParentScope, Program)
        {
            Type = new TypeType(ParentScope, Program)
            {
                Name = Name,
                Members = TypeBody.InstanceDefinitions
            },
            Name = Name,
            SourceRange = SourceRange,
            References = new List<SourceRange> {SourceRange},
            Attributes = Definition.Attribute.Type
        });
    }
}
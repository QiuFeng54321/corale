using PseudoCode.Runtime.Operations;

namespace PseudoCode.Runtime;

public class ReferenceInstance : Instance
{
    public uint ReferenceAddress { get; set; }

    public Instance ReferencedInstance => Program.Memory[ReferenceAddress];

    public override Dictionary<string, Instance> Members => ReferencedInstance.Members;
    public override Type Type => ReferencedInstance.Type;

    public override object Value
    {
        get => ReferencedInstance.Value;
        set => ReferencedInstance.Value = value;
    }

    public override string Represent() => ReferencedInstance.Represent();

    public override string DebugRepresent() => $"Ref {ReferencedInstance.DebugRepresent()}";
    

    public ReferenceInstance(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }
}
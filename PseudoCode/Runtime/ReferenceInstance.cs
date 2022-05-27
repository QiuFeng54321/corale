using PseudoCode.Runtime.Operations;

namespace PseudoCode.Runtime;

public class ReferenceInstance : Instance
{
    public ReferenceInstance(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public uint ReferenceAddress { get; set; }

    public override Instance RealInstance => Program.Memory[ReferenceAddress];


    public override Dictionary<string, Instance> Members => RealInstance.Members;
    public override Type Type => RealInstance.Type;

    public override object Value
    {
        get => RealInstance.Value;
        set => RealInstance.Value = value;
    }

    public override string Represent()
    {
        return RealInstance.Represent();
    }

    public override string DebugRepresent()
    {
        return $"Ref {RealInstance.DebugRepresent()}";
    }
}
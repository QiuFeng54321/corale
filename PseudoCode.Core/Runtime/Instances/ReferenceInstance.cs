using Newtonsoft.Json;
using PseudoCode.Core.Runtime.Operations;
using Type = PseudoCode.Core.Runtime.Types.Type;

namespace PseudoCode.Core.Runtime.Instances;

public class ReferenceInstance : Instance
{
    public ReferenceInstance(Scope parentScope, PseudoProgram program) : base(parentScope, program)
    {
    }

    public uint ReferenceAddress { get; set; }

    public override Instance RealInstance => Program.Memory[ReferenceAddress];


    [JsonIgnore] public override Dictionary<string, Instance> Members => RealInstance.Members;
    [JsonIgnore] public override Type Type => RealInstance.Type;

    [JsonIgnore]
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
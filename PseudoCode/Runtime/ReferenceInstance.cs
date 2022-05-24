using PseudoCode.Runtime.Operations;

namespace PseudoCode.Runtime;

public class ReferenceInstance : Instance
{
    public string ReferenceName;
    public Scope Scope;

    public Instance ReferencedInstance => Scope.FindInstance(ReferenceName);

    public override Dictionary<string, Instance> Members => ReferencedInstance.Members;
    public override Type Type => ReferencedInstance.Type;

    public override object Value
    {
        get => ReferencedInstance.Value;
        set => ReferencedInstance.Value = value;
    }


    public override string ToString()
    {
        return $"Ref {base.ToString()}";
    }
}
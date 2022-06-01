namespace PseudoCode.Core.Runtime.Operations;

public class ScopeStates : ICloneable
{
    public Dictionary<string, uint> InstanceAddresses = new();
    public List<Operation> Operations = new();
    public Dictionary<string, Type> Types = new();

    public void ResetTemporaryContent()
    {
        InstanceAddresses = new Dictionary<string, uint>();
        Types = new Dictionary<string, Type>();
    }

    public object Clone()
    {
        return new ScopeStates
        {
            InstanceAddresses = new Dictionary<string, uint>(InstanceAddresses),
            Operations = new List<Operation>(Operations),
            Types = new Dictionary<string, Type>(Types)
        };
    }
}
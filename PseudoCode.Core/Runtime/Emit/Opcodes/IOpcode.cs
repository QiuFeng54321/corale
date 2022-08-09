namespace PseudoCode.Core.Runtime.Emit.Opcodes;

public interface IOpcode
{
    public Label Label { get; set; }
    public void Execute(PseudoRuntime runtime);
    public string Represent();

    public string ToString()
    {
        return $"{Label,10} {Represent()}";
    }
}

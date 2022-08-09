namespace PseudoCode.Core.Runtime.Emit.Opcodes;

public class Assign : IOpcode
{
    public Label Label { get; set; }
    public void Execute(PseudoRuntime runtime)
    {
        throw new NotImplementedException();
    }

    public string Represent() => "ASSIGN";
}
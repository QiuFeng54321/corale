namespace PseudoCode.Core.Runtime.Emit.Opcodes;

public class Nop : IOpcode
{
    public Label Label { get; set; }
    public void Execute(PseudoRuntime runtime)
    {
        
    }

    public string Represent() => "NOP";
}
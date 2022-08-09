namespace PseudoCode.Core.Runtime.Emit.Opcodes;

public class Goto : IOpcode
{
    public Label Label { get; set; }
    public Label ToLabel { get; set; }
    public void Execute(PseudoRuntime runtime)
    {
        runtime.ProgramCounter = ToLabel.OpcodeIndex;
    }

    public string Represent() => $"GOTO {ToLabel}";
}
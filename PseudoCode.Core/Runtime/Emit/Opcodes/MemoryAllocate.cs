using System.Runtime.InteropServices;

namespace PseudoCode.Core.Runtime.Emit.Opcodes;

public class MemoryAllocate : IOpcode
{
    public Label Label { get; set; }
    public void Execute(PseudoRuntime runtime)
    {
        throw new NotImplementedException();
    }

    public string Represent()
    {
        throw new NotImplementedException();
    }
}
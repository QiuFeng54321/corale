using PseudoCode.Core.Runtime.Instances;

namespace PseudoCode.Core.Runtime.Emit;

public class PseudoRuntime
{
    public PseudoAssembly PseudoAssembly;
    public long ProgramCounter;
    public Stack<Instance> RuntimeStack = new();
    public Instance[] Memory;
    public void PushInstance(Instance instance) => RuntimeStack.Push(instance);

    public Instance GetInstanceInMemory(MemoryAddress address)
    {
        return Memory[address];
    }
    public Instance PopInstance()
    {
        return RuntimeStack.Count > 0 ? RuntimeStack.Pop() : null;
    }

    public void Execute()
    {
        ProgramCounter = 0;
        Memory = new Instance[1024]; // TODO: Make the size variable
        while (ProgramCounter < PseudoAssembly.Opcodes.Length)
        {
            PseudoAssembly.Opcodes[ProgramCounter].Execute(this);
        }
    }
}
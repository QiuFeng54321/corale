using PseudoCode.Core.Analyzing;
using PseudoCode.Core.Runtime.Operations;

namespace PseudoCode.Core.Runtime;

public class PseudoProgram
{
    public uint CurrentInstanceAddress;
    public bool DebugRepresentation;
    public bool DisplayOperationsAfterCompiled { get; set; }
    public bool DisplayOperationsAtRuntime { get; set; }
    public Scope GlobalScope;
    public Dictionary<uint, Instance> Memory = new();
    public Stack<Instance> RuntimeStack = new();
    public Stack<Type> TypeCheckStack = new();
    public List<Feedback> AnalyserFeedbacks = new();
    public bool AllowUndeclaredVariables { get; set; }

    public PseudoProgram()
    {
        GlobalScope = new Scope(null, this);
        AddPrimitiveTypes();
    }

    public uint AllocateId(Instance i)
    {
        i.InstanceAddress = CurrentInstanceAddress++;
        i.Program = this;
        Memory.Add(i.InstanceAddress, i);
        return i.InstanceAddress;
    }

    public uint AllocateId(Func<Instance> generator)
    {
        return AllocateId(generator());
    }

    public uint Allocate(int length, Func<Instance> generator)
    {
        var startAddress = CurrentInstanceAddress;
        for (var i = 0; i < length; i++) AllocateId(generator);

        return startAddress;
    }

    public void SetMemory(Range segment, Func<Instance> value)
    {
        for (var i = (uint)segment.Start; i < segment.End; i++)
        {
            Memory[i] = value();
            Memory[i].InstanceAddress = i;
            Memory[i].Program = this;
        }
    }

    public void ReleaseMemory(Range segment)
    {
        for (var i = (uint)segment.Start; i < segment.End; i++)
            if (Memory.ContainsKey(i))
                Memory.Remove(i);
    }

    public void AddPrimitiveTypes()
    {
        GlobalScope.AddType(new BooleanType (GlobalScope, this));
        GlobalScope.AddType(new IntegerType (GlobalScope, this));
        GlobalScope.AddType(new RealType (GlobalScope, this));
        GlobalScope.AddType(new StringType (GlobalScope, this));
        GlobalScope.AddType(new CharacterType (GlobalScope, this));
        GlobalScope.AddType(new DateType (GlobalScope, this));
        GlobalScope.AddType(new NullType (GlobalScope, this));
        GlobalScope.AddType(new PlaceholderType (GlobalScope, this));
        Instance.Null = GlobalScope.FindTypeDefinition(Type.NullId).Type.Instance(scope: GlobalScope);
    }
}
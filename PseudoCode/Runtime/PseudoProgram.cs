using PseudoCode.Runtime.Operations;

namespace PseudoCode.Runtime;

public class PseudoProgram
{
    public uint CurrentInstanceAddress;
    public bool DebugRepresentation;
    public bool DisplayOperations { get; set; }
    public Scope GlobalScope;
    public Dictionary<uint, Instance> Memory = new();
    public Stack<Instance> RuntimeStack = new();
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
            if (Memory.ContainsKey(i)) Memory.Remove(i);
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
        GlobalScope.AddType("BOOLEAN", new BooleanType { ParentScope = GlobalScope, Program = this });
        GlobalScope.AddType("INTEGER", new IntegerType { ParentScope = GlobalScope, Program = this });
        GlobalScope.AddType("REAL", new RealType { ParentScope = GlobalScope, Program = this });
        GlobalScope.AddType("ARRAY", new ArrayType { ParentScope = GlobalScope, Program = this });
        GlobalScope.AddType("STRING", new StringType { ParentScope = GlobalScope, Program = this });
        GlobalScope.AddType("CHAR", new CharacterType { ParentScope = GlobalScope, Program = this });
        GlobalScope.AddType("DATE", new DateType { ParentScope = GlobalScope, Program = this });
        GlobalScope.AddType("NULL", new NullType { ParentScope = GlobalScope, Program = this });
        GlobalScope.AddType("PLACEHOLDER", new PlaceholderType { ParentScope = GlobalScope, Program = this });
        Instance.Null = GlobalScope.FindType(Type.NullId).Instance(scope: GlobalScope);
    }
}
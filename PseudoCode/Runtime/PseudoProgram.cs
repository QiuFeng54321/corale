using PseudoCode.Runtime.Operations;

namespace PseudoCode.Runtime;

public class PseudoProgram
{
    public uint CurrentInstanceAddress;
    public Scope GlobalScope;
    public Dictionary<uint, Instance> Memory = new();

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
        GlobalScope.AddType("BOOLEAN", new BooleanType { Scope = GlobalScope, Program = this });
        GlobalScope.AddType("INTEGER", new IntegerType { Scope = GlobalScope, Program = this });
        GlobalScope.AddType("REAL", new RealType { Scope = GlobalScope, Program = this });
        GlobalScope.AddType("ARRAY", new ArrayType { Scope = GlobalScope, Program = this });
        GlobalScope.AddType("STRING", new StringType { Scope = GlobalScope, Program = this });
        GlobalScope.AddType("CHAR", new CharacterType { Scope = GlobalScope, Program = this });
        GlobalScope.AddType("DATE", new DateType { Scope = GlobalScope, Program = this });
        GlobalScope.AddType("NULL", new NullType { Scope = GlobalScope, Program = this });
        GlobalScope.AddType("PLACEHOLDER", new PlaceholderType { Scope = GlobalScope, Program = this });
        Instance.Null = GlobalScope.FindType(Type.NullId).Instance();
    }
}
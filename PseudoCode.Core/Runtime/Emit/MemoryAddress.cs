namespace PseudoCode.Core.Runtime.Emit;

public class MemoryAddress
{
    public readonly ulong Address;

    public MemoryAddress(ulong address)
    {
        Address = address;
    }

    public static implicit operator MemoryAddress(ulong address)
    {
        return new MemoryAddress(address);
    }

    public static implicit operator ulong(MemoryAddress memoryAddress)
    {
        return memoryAddress.Address;
    }
}
using System.Runtime.InteropServices;

namespace PseudoCode.Core.Runtime.Reflection;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
public struct BlittableSByte
{
    public sbyte Value;

    public static explicit operator BlittableSByte(sbyte value)
    {
        return new BlittableSByte { Value = value };
    }

    public static implicit operator sbyte(BlittableSByte value)
    {
        return value.Value;
    }
}
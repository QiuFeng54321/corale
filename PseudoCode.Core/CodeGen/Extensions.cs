using System.Runtime.InteropServices;

namespace PseudoCode.Core.CodeGen;

public static class Extensions
{
    public static unsafe sbyte* ToSByte(this string str)
    {
        return (sbyte*)Marshal.StringToHGlobalAuto(str);
    }
}
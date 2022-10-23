using System.Runtime.InteropServices;
using System.Text;
using PseudoCode.Core.CodeGen;

namespace PseudoCode.Core.Runtime.Reflection.Builtin;

[StructLayout(LayoutKind.Sequential)]
[BuiltinType("STRING")]
public struct PseudoStringStruct
{
    public unsafe sbyte* Pointer;
    public int Length;

    public override unsafe string ToString()
    {
        return new(Pointer, 0, Length);
    }

    public static unsafe implicit operator PseudoStringStruct(string value)
    {
        return new PseudoStringStruct
        {
            Pointer = value.ToSByte(),
            Length = Encoding.UTF8.GetByteCount(value) // sbyte length is different from char length
        };
    }

    public static unsafe implicit operator PseudoStringStruct(sbyte* value)
    {
        var str = new string(value);
        return str;
    }

    public static implicit operator string(PseudoStringStruct value)
    {
        return value.ToString();
    }
}
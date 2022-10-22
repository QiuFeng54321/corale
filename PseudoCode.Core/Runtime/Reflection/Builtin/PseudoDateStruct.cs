using System.Runtime.InteropServices;

namespace PseudoCode.Core.Runtime.Reflection.Builtin;

[StructLayout(LayoutKind.Sequential)]
[BuiltinType("DATE")]
public struct PseudoDateStruct
{
    public int Year;
    public int Month;
    public int Day;
}
using System.Runtime.InteropServices;

namespace PseudoCode.Core.Runtime.Reflection.Builtin;

[StructLayout(LayoutKind.Sequential)]
[BuiltinType("STRING")]
public struct PseudoStringStruct
{
    public unsafe char* CStr;
    public int Length;
}
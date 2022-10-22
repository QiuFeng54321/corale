using System.Runtime.InteropServices;

namespace PseudoCode.Core.Runtime.Reflection.Builtin;

[StructLayout(LayoutKind.Sequential)]
public struct PseudoStringStruct
{
    public unsafe char* CStr;
    public int Length;
}
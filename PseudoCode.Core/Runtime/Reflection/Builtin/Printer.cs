using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace PseudoCode.Core.Runtime.Reflection.Builtin;

public static class Printer
{
    [BuiltinNativeFunction("__PRINTF")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static void Printf(int x)
    {
        Console.Write(x);
    }

    [BuiltinNativeFunction("__PRINTF")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static void Printf(char x)
    {
        Console.Write(x);
    }

    [BuiltinNativeFunction("__PRINTF")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static void Printf(double x)
    {
        Console.Write(x);
    }

    [BuiltinNativeFunction("__PRINTF")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static void Printf(BlittableBoolean x)
    {
        Console.Write(x ? "TRUE" : "FALSE");
    }

    [BuiltinNativeFunction("__PRINTLN")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static void Println()
    {
        Console.WriteLine();
    }
}
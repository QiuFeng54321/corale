using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace PseudoCode.Core.Runtime.Reflection.Builtin;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
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
    public static void Printf(sbyte x)
    {
        Console.Write((char)x);
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

    [BuiltinNativeFunction("__PRINTF")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static void Printf(PseudoStringStruct x)
    {
        Console.Write(x);
    }

    [BuiltinNativeFunction("__PRINTF")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe void Printf(sbyte* x)
    {
        Console.Write(new string(x));
    }

    [BuiltinNativeFunction("__PRINTF")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static void Printf(PseudoDateStruct x)
    {
        Console.Write($"{x.Month:00}/{x.Day:00}/{x.Year:0000}");
    }

    [BuiltinNativeFunction("__PRINTLN")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static void Println()
    {
        Console.WriteLine();
    }
}
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace PseudoCode.Core.Runtime.Reflection.Builtin;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class Scanner
{
    [BuiltinNativeFunction("__SCAN")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe void Scan([ByRef] int* x)
    {
        *x = int.Parse(Console.ReadLine()!);
    }

    [BuiltinNativeFunction("__SCAN")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe void Scan([ByRef] double* x)
    {
        *x = double.Parse(Console.ReadLine()!);
    }

    [BuiltinNativeFunction("__SCAN")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe void Scan([ByRef] BlittableBoolean* x)
    {
        *x = (BlittableBoolean)bool.Parse(Console.ReadLine()!);
    }

    [BuiltinNativeFunction("__SCAN")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe void Scan([ByRef] sbyte* x)
    {
        *x = (sbyte)Console.Read();
    }

    [BuiltinNativeFunction("__SCAN")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe void Scan([ByRef] PseudoStringStruct* x)
    {
        *x = Console.ReadLine();
    }

    [BuiltinNativeFunction("__SCAN")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe void Scan([ByRef] PseudoDateStruct* x)
    {
        // ReSharper disable once StringLiteralTypo
        *x = DateOnly.ParseExact(Console.ReadLine()!, "MM/dd/yyyy");
    }
}
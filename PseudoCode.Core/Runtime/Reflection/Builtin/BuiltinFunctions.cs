using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using PseudoCode.Core.Runtime.Errors;

namespace PseudoCode.Core.Runtime.Reflection.Builtin;

public static class BuiltinFunctions
{
    public static readonly Random Random = new();


    // [BuiltinFunction("EOF")]
    // [ParamType("path", "STRING")]
    // [ReturnType("BOOLEAN")]
    // public static Instance Eof(Scope parentScope, PseudoProgram program, Instance[] arguments)
    // {
    //     var path = arguments[0].Get<string>();
    //     return parentScope.FindDefinition(Type.BooleanId).Type.Instance(program.OpenFiles[path].Eof());
    // }

    [BuiltinNativeFunction("RIGHT")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static PseudoStringStruct Right(PseudoStringStruct thisString, long x)
    {
        if (x < 0 || x > thisString.Length)
            throw new OutOfBoundsError(
                $"Cannot take substring [^{x}..] of \"{thisString}\": Length of string is {thisString.Length}");

        return thisString.ToString()[^(int)x..];
    }

    [BuiltinNativeFunction("LEFT")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static PseudoStringStruct Left(PseudoStringStruct str, long index)
    {
        if (index < 0 || index > str.Length)
            throw new OutOfBoundsError(
                $"Cannot take substring [..{index}] of \"{str}\": Length of string is {str.Length}"
            );

        return str.ToString()[..(int)index];
    }


    [BuiltinNativeFunction("LENGTH")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static long Length(PseudoStringStruct thisString)
    {
        return thisString.Length;
    }

    [BuiltinNativeFunction("__in_range")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static bool InRange(RealNumberType target, RealNumberType from, RealNumberType to)
    {
        return from <= target && target <= to;
    }

    [BuiltinNativeFunction("MID")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static PseudoStringStruct Mid(PseudoStringStruct thisString, long x, long y)
    {
        if (x < 0 || x >= thisString.Length || y < 0 || x + y >= thisString.Length)
            throw new OutOfBoundsError(
                $"Cannot take substring [{x}..{x + y}] of \"{thisString}\": Length of string is {thisString.Length}",
                null);
        return thisString.ToString().Substring((int)x - 1, (int)y);
    }

    [BuiltinNativeFunction("LCASE")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static sbyte LowerCase(sbyte thisChar)
    {
        return (sbyte)char.ToLower((char)thisChar);
    }

    [BuiltinNativeFunction("UCASE")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static sbyte UpperCase(sbyte thisChar)
    {
        return (sbyte)char.ToUpper((char)thisChar);
    }

    [BuiltinNativeFunction("TO_UPPER")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static PseudoStringStruct ToUpper(PseudoStringStruct str)
    {
        return str.ToString().ToUpper();
    }

    [BuiltinNativeFunction("TO_LOWER")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static PseudoStringStruct ToLower(PseudoStringStruct str)
    {
        return str.ToString().ToLower();
    }

    [BuiltinNativeFunction("NUM_TO_STR")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static PseudoStringStruct NumToStr(double num)
    {
        return num.ToString();
    }

    [BuiltinNativeFunction("STR_TO_NUM")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static double StrToNum(PseudoStringStruct str)
    {
        return double.Parse(str);
    }

    [BuiltinNativeFunction("IS_NUM")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static bool IsNum(PseudoStringStruct str)
    {
        return double.TryParse(str, out _);
    }

    [BuiltinNativeFunction("ASC")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static long Ascii(sbyte chr)
    {
        return chr;
    }

    [BuiltinNativeFunction("CHR")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static sbyte Char(long ascii)
    {
        return (sbyte)ascii;
    }

    [BuiltinNativeFunction("DAY")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static long Day(PseudoDateStruct date)
    {
        return date.Day;
    }

    [BuiltinNativeFunction("MONTH")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static long Month(PseudoDateStruct date)
    {
        return date.Month;
    }

    [BuiltinNativeFunction("YEAR")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static long Year(PseudoDateStruct date)
    {
        return date.Year;
    }

    [BuiltinNativeFunction("DAYINDEX")]
    public static long DayIndex(DateOnly date)
    {
        return (long)date.DayOfWeek + 1;
    }

    [BuiltinNativeFunction("SETDATE")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static PseudoDateStruct SetDate(long day, long month, long year)
    {
        return new PseudoDateStruct(year, month, day);
    }

    [BuiltinNativeFunction("TODAY")]
    public static DateOnly Today() => DateOnly.FromDateTime(DateTime.Today);

    [BuiltinNativeFunction("INT")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static long Int(double x)
    {
        return (long)x;
    }

    [BuiltinNativeFunction("FIND")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static long Find(PseudoStringStruct str, sbyte chr)
    {
        return str.ToString().IndexOf((char)chr) + 1;
    }

    [BuiltinNativeFunction("RAND")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static double RandomReal(long x)
    {
        // ReSharper disable once RedundantCast
        return (RealNumberType)(Random.NextDouble() * x);
    }

    [BuiltinNativeFunction("__STR")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe PseudoStringStruct Str(sbyte* x)
    {
        return x;
    }
}
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using PseudoCode.Core.Runtime.Errors;

namespace PseudoCode.Core.Runtime.Reflection.Builtin;

public static class BuiltinFunctions
{
    public static readonly Random Random = new();

    [Documentation("Returns a string containing `x` characters from the right of the source string")]
    [BuiltinNativeFunction("RIGHT")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static PseudoStringStruct Right(PseudoStringStruct thisString, long x)
    {
        if (x < 1 || x > thisString.Length)
            throw new OutOfBoundsError(
                $"Cannot take substring [^{x}..] of \"{thisString}\": Length of string is {thisString.Length}");

        return thisString.ToString()[^(int)x..];
    }

    [Documentation("Returns a string containing `x` characters from the start of the source string")]
    [BuiltinNativeFunction("LEFT")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static PseudoStringStruct Left(PseudoStringStruct str, long index)
    {
        if (index < 1 || index > str.Length)
            throw new OutOfBoundsError(
                $"Cannot take substring [..{index}] of \"{str}\": Length of string is {str.Length}"
            );

        return str.ToString()[..(int)index];
    }


    [Documentation("Returns the length of the string")]
    [BuiltinNativeFunction("LENGTH")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static long Length(PseudoStringStruct thisString)
    {
        return thisString.Length;
    }

    [Documentation("Returns if `target` falls between the bounds inclusively.\n**You shouldn't use this**")]
    [BuiltinNativeFunction("__in_range")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static bool InRange(RealNumberType target, RealNumberType from, RealNumberType to)
    {
        return from <= target && target <= to;
    }

    [Documentation("Returns the substring of `length` of the source string starting from `index`")]
    [BuiltinNativeFunction("MID")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static PseudoStringStruct Mid(PseudoStringStruct thisString, long index, long length)
    {
        if (index < 1 || index > thisString.Length || length < 1 || index + length - 1 > thisString.Length)
            throw new OutOfBoundsError(
                $"Cannot take substring [{index}..{index + length}] of \"{thisString}\": Length of string is {thisString.Length}",
                null);
        return thisString.ToString().Substring((int)index - 1, (int)length);
    }

    [Documentation("Turns the character into lower case.\n`'C' -> 'c'`\n`'c' -> 'c'`")]
    [BuiltinNativeFunction("LCASE")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static sbyte LowerCase(sbyte thisChar)
    {
        return (sbyte)char.ToLower((char)thisChar);
    }

    [Documentation("Turns the character into upper case.\n`'C' -> 'C'`\n`'c' -> 'C'`")]
    [BuiltinNativeFunction("UCASE")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static sbyte UpperCase(sbyte thisChar)
    {
        return (sbyte)char.ToUpper((char)thisChar);
    }

    [Documentation("Turns all the characters in the string into upper case.\n`\"aBcD\" -> \"ABCD\"`")]
    [BuiltinNativeFunction("TO_UPPER")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static PseudoStringStruct ToUpper(PseudoStringStruct str)
    {
        return str.ToString().ToUpper();
    }

    [Documentation("Turns all the characters in the string into lower case.\n`\"aBcD\" -> \"abcd\"`")]
    [BuiltinNativeFunction("TO_LOWER")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static PseudoStringStruct ToLower(PseudoStringStruct str)
    {
        return str.ToString().ToLower();
    }

    [Documentation("Converts a number into a string")]
    [BuiltinNativeFunction("NUM_TO_STR")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static PseudoStringStruct NumToStr(double num)
    {
        return num.ToString();
    }

    [Documentation("Converts a string into a number")]
    [BuiltinNativeFunction("STR_TO_NUM")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static double StrToNum(PseudoStringStruct str)
    {
        return double.Parse(str);
    }

    [Documentation("Returns if the given string can be parsed as a number")]
    [BuiltinNativeFunction("IS_NUM")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static bool IsNum(PseudoStringStruct str)
    {
        return double.TryParse(str, out _);
    }

    [Documentation("Returns the ascii index of a character")]
    [BuiltinNativeFunction("ASC")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static long Ascii(sbyte chr)
    {
        return chr;
    }

    [Documentation("Returns the character from the given ascii index")]
    [BuiltinNativeFunction("CHR")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static sbyte Char(long ascii)
    {
        return (sbyte)ascii;
    }

    [Documentation("Returns the day component of a date")]
    [BuiltinNativeFunction("DAY")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static long Day(PseudoDateStruct date)
    {
        return date.Day;
    }

    [Documentation("Returns the month component of a date")]
    [BuiltinNativeFunction("MONTH")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static long Month(PseudoDateStruct date)
    {
        return date.Month;
    }

    [Documentation("Returns the year component of a date")]
    [BuiltinNativeFunction("YEAR")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static long Year(PseudoDateStruct date)
    {
        return date.Year;
    }

    [Documentation("Returns the day of a date (`Sunday` = 1, `Saturday` = 7)")]
    [BuiltinNativeFunction("DAYINDEX")]
    public static long DayIndex(DateOnly date)
    {
        return (long)date.DayOfWeek + 1;
    }

    [Documentation("Constructs a date using given `day`, `month`, and `year`")]
    [BuiltinNativeFunction("SETDATE")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static PseudoDateStruct SetDate(long day, long month, long year)
    {
        return new PseudoDateStruct(year, month, day);
    }

    [Documentation("Returns the date of today")]
    [BuiltinNativeFunction("TODAY")]
    public static DateOnly Today() => DateOnly.FromDateTime(DateTime.Today);

    [Documentation("Returns the integer component of a `REAL` number")]
    [BuiltinNativeFunction("INT")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static long Int(double x)
    {
        return (long)x;
    }

    [Documentation("Returns the index of the first occasion of the character in the string")]
    [BuiltinNativeFunction("FIND")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static long Find(PseudoStringStruct str, sbyte chr)
    {
        return str.ToString().IndexOf((char)chr) + 1;
    }

    [Documentation("Returns the index of the first occasion of the character in the string starting from `index`")]
    [BuiltinNativeFunction("INSTR")]
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static long InString(long index, PseudoStringStruct str, sbyte chr)
    {
        return str.ToString()[(int)(index + 1)..].IndexOf((char)chr) + index;
    }

    [Documentation("Returns a random number [0..x)")]
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
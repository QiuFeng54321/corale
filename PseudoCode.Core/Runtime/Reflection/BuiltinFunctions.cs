using PseudoCode.Core.Runtime.Errors;
using PseudoCode.Core.Runtime.Instances;
using PseudoCode.Core.Runtime.Operations;
using Type = PseudoCode.Core.Runtime.Types.Type;

namespace PseudoCode.Core.Runtime.Reflection;

public static class BuiltinFunctions
{
    public static Random Random = new();

    [BuiltinFunction("EOF")]
    [ParamType("path", "STRING")]
    [ReturnType("BOOLEAN")]
    public static Instance Eof(Scope parentScope, PseudoProgram program, Instance[] arguments)
    {
        var path = arguments[0].Get<string>();
        return parentScope.FindDefinition(Type.BooleanId).Type.Instance(program.OpenFiles[path].Eof());
    }

    [BuiltinNativeFunction("RIGHT")]
    public static string Right(string thisString, int x)
    {
        if (x < 0 || x > thisString.Length)
            throw new OutOfBoundsError($"Cannot take substring [^{x}..] of \"{thisString}\": Length of string is {thisString.Length}", null);

        return thisString[^x..];
    }

    [BuiltinNativeFunction("LEFT")]
    public static string Left(string str, int index)
    {
        if (index < 0 || index > str.Length)
            throw new OutOfBoundsError(
                $"Cannot take substring [..{index}] of \"{str}\": Length of string is {str.Length}",
                null);

        return str[..index];
    }


    [BuiltinNativeFunction("LENGTH")]
    public static int Length(string thisString)
    {
        return thisString.Length;
    }

    [BuiltinNativeFunction("__in_range")]
    public static bool InRange(RealNumberType target, RealNumberType from, RealNumberType to)
    {
        return from <= target && target <= to;
    }

    [BuiltinNativeFunction("MID")]
    public static string Mid(string thisString, int x, int y)
    {
        if (x < 0 || x >= thisString.Length || y < 0 || x + y >= thisString.Length)
            throw new OutOfBoundsError(
                $"Cannot take substring [{x}..{x + y}] of \"{thisString}\": Length of string is {thisString.Length}",
                null);
        return thisString.Substring(x - 1, y);
    }

    [BuiltinNativeFunction("LCASE")]
    public static char LowerCase(char thisChar)
    {
        return char.ToLower(thisChar);
    }

    [BuiltinNativeFunction("UCASE")]
    public static char UpperCase(char thisChar)
    {
        return char.ToUpper(thisChar);
    }

    [BuiltinNativeFunction("TO_UPPER")]
    public static string ToUpper(string str) => str.ToUpper();

    [BuiltinNativeFunction("TO_LOWER")]
    public static string ToLower(string str) => str.ToLower();

    [BuiltinNativeFunction("NUM_TO_STR")]
    public static string NumToStr(RealNumberType num) => num.ToString();

    [BuiltinNativeFunction("STR_TO_NUM")]
    public static RealNumberType StrToNum(string str) => RealNumberType.Parse(str);

    [BuiltinNativeFunction("IS_NUM")]
    public static bool IsNum(string str) => RealNumberType.TryParse(str, out _);

    [BuiltinNativeFunction("ASC")]
    public static int Ascii(char chr) => chr;

    [BuiltinNativeFunction("CHR")]
    public static char Char(int ascii) => (char)ascii;

    [BuiltinNativeFunction("DAY")]
    public static int Day(DateOnly date) => date.Day;

    [BuiltinNativeFunction("MONTH")]
    public static int Month(DateOnly date) => date.Month;

    [BuiltinNativeFunction("YEAR")]
    public static int Year(DateOnly date) => date.Year;

    [BuiltinNativeFunction("DAYINDEX")]
    public static int DayIndex(DateOnly date) => (int)date.DayOfWeek + 1;

    [BuiltinNativeFunction("SETDATE")]
    public static DateOnly SetDate(int day, int month, int year)
    {
        return new DateOnly(year, month, day);
    }

    [BuiltinNativeFunction("TODAY")]
    public static DateOnly Today() => DateOnly.FromDateTime(DateTime.Today);

    [BuiltinNativeFunction("INT")]
    public static int Int(RealNumberType x)
    {
        return (int)x;
    }

    [BuiltinNativeFunction("FIND")]
    public static int Find(string str, char chr)
    {
        return str.IndexOf(chr) + 1;
    }

    [BuiltinNativeFunction("RAND")]
    public static RealNumberType RandomReal(int x)
    {
        // ReSharper disable once RedundantCast
        return (RealNumberType)(Random.NextDouble() * x);
    }
}